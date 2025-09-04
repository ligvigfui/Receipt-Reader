from fastapi import FastAPI, File, UploadFile
from fastapi.responses import JSONResponse
from typing import Any
import uvicorn
from PIL import Image
import io
import torch
from donut import DonutModel

app = FastAPI()

# Load the pretrained Donut model once at startup
MODEL_PATH = './donut-model'  # Path to the downloaded model
model = DonutModel.from_pretrained("naver-clova-ix/donut-base-finetuned-rvlcdip")
device = 'cuda' if torch.cuda.is_available() else 'cpu'
model.to(device)
model.eval()

@app.post("/extract")
async def extract_from_image(file: UploadFile = File(...)):
    image_bytes = await file.read()
    image = Image.open(io.BytesIO(image_bytes)).convert('RGB')
    # Donut expects a list of images
    with torch.no_grad():
        result = model.inference([image], prompt=None, return_json=True)
    return JSONResponse(content=result)

if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)
