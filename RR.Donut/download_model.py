"""
download_model.py - Download a pretrained Donut model and save it locally.

Usage:
    python download_model.py --model naver-clova-ix/donut-base-finetuned-docvqa --output_dir ./donut-model
"""
import argparse
from donut import DonutModel

def main():
    parser = argparse.ArgumentParser(description="Download a pretrained Donut model.")
    parser.add_argument('--model', type=str, default='naver-clova-ix/donut-base-finetuned-docvqa', help='Model name or path')
    parser.add_argument('--output_dir', type=str, default='./donut-model', help='Directory to save the model')
    args = parser.parse_args()

    print(f"Downloading model: {args.model}")
    model = DonutModel.from_pretrained(args.model)
    model.save_pretrained(args.output_dir)
    print(f"Model saved to {args.output_dir}")

if __name__ == "__main__":
    main()
