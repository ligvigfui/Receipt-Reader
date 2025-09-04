# RR.Donut FastAPI OCR Service

## Setup

1. (Recommended) Create and activate a virtual environment:
   ```sh
   python -m venv .venv
   source .venv/bin/activate  # On Windows: .venv\Scripts\activate
   ```

2. Install dependencies:
   ```sh
   pip install fastapi uvicorn[standard] torch torchvision
   pip install git+https://github.com/clovaai/donut.git
   pip install pillow
   ```

3. Download a pretrained Donut model (for example, the demo model):
   ```python
   # In Python REPL or a script
   from donut import DonutModel
   model = DonutModel.from_pretrained('naver-clova-ix/donut-base-finetuned-docvqa')
   model.save_pretrained('./donut-model')
   ```

4. Run the API:
   ```sh
   uvicorn main:app --reload
   ```

## Usage

Send a POST request to `/extract` with a PNG or JPG file as form-data under the `file` key. The response will be a JSON with the extracted data.


## Error I got

```
Traceback (most recent call last):
  File "D:\programming\Receipt-Reader\RR.Donut\main.py", line 14, in <module>
    model = DonutModel.from_pretrained("naver-clova-ix/donut-base-finetuned-rvlcdip")
  File "D:\programming\Receipt-Reader\RR.Donut\.venv\Lib\site-packages\donut\model.py", line 597, in from_pretrained
    model = super(DonutModel, cls).from_pretrained(pretrained_model_name_or_path, revision="official", *model_args, **kwargs)
  File "D:\programming\Receipt-Reader\RR.Donut\.venv\Lib\site-packages\transformers\modeling_utils.py", line 288, in _wrapper
    return func(*args, **kwargs)
  File "D:\programming\Receipt-Reader\RR.Donut\.venv\Lib\site-packages\transformers\modeling_utils.py", line 5176, in from_pretrained
    ) = cls._load_pretrained_model(
        ~~~~~~~~~~~~~~~~~~~~~~~~~~^
        model,
        ^^^^^^
    ...<13 lines>...
        weights_only=weights_only,
        ^^^^^^^^^^^^^^^^^^^^^^^^^^
    )
    ^
  File "D:\programming\Receipt-Reader\RR.Donut\.venv\Lib\site-packages\transformers\modeling_utils.py", line 5639, in _load_pretrained_model
    _error_msgs, disk_offload_index, cpu_offload_index = load_shard_file(args)
                                                         ~~~~~~~~~~~~~~~^^^^^^
  File "D:\programming\Receipt-Reader\RR.Donut\.venv\Lib\site-packages\transformers\modeling_utils.py", line 946, in load_shard_file
    disk_offload_index, cpu_offload_index = _load_state_dict_into_meta_model(
                                            ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~^
        model_to_load,
        ^^^^^^^^^^^^^^
    ...<13 lines>...
        device_mesh=device_mesh,
        ^^^^^^^^^^^^^^^^^^^^^^^^
    )
    ^
  File "D:\programming\Receipt-Reader\RR.Donut\.venv\Lib\site-packages\torch\utils\_contextlib.py", line 120, in decorate_context
    return func(*args, **kwargs)
  File "D:\programming\Receipt-Reader\RR.Donut\.venv\Lib\site-packages\transformers\modeling_utils.py", line 850, in _load_state_dict_into_meta_model
    _load_parameter_into_model(model, param_name, param.to(param_device))
    ~~~~~~~~~~~~~~~~~~~~~~~~~~^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
  File "D:\programming\Receipt-Reader\RR.Donut\.venv\Lib\site-packages\transformers\modeling_utils.py", line 710, in _load_parameter_into_model
    module.load_state_dict({param_type: tensor}, strict=False, assign=True)
    ~~~~~~~~~~~~~~~~~~~~~~^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
  File "D:\programming\Receipt-Reader\RR.Donut\.venv\Lib\site-packages\torch\nn\modules\module.py", line 2624, in load_state_dict
    raise RuntimeError(
    ...<3 lines>...
    )
RuntimeError: Error(s) in loading state_dict for Linear:
        size mismatch for weight: copying a param with shape torch.Size([512, 1024]) from checkpoint, the shape in current model is torch.Size([256, 512]).
```