"""
finetune.py - Fine-tune a Donut model on your own dataset

Usage:
    python finetune.py --train_dir ./data/train --val_dir ./data/val --output_dir ./donut-finetuned --pretrained_model naver-clova-ix/donut-base-finetuned-docvqa

Requirements:
    - torch, torchvision, donut, transformers, datasets
    - Your data should be organized as images and corresponding JSON labels.
"""
import argparse
import os
from donut import DonutModel
from datasets import load_dataset, DatasetDict
from transformers import TrainingArguments, Trainer
import torch

def parse_args():
    parser = argparse.ArgumentParser(description="Fine-tune Donut on your dataset.")
    parser.add_argument('--train_dir', type=str, required=True, help='Path to training images and labels')
    parser.add_argument('--val_dir', type=str, required=True, help='Path to validation images and labels')
    parser.add_argument('--output_dir', type=str, required=True, help='Where to save the fine-tuned model')
    parser.add_argument('--pretrained_model', type=str, default='naver-clova-ix/donut-base-finetuned-docvqa', help='Pretrained Donut model')
    parser.add_argument('--epochs', type=int, default=5, help='Number of epochs')
    parser.add_argument('--batch_size', type=int, default=2, help='Batch size')
    return parser.parse_args()

def main():
    args = parse_args()
    device = 'cuda' if torch.cuda.is_available() else 'cpu'

    print(f"Loading model: {args.pretrained_model}")
    model = DonutModel.from_pretrained(args.pretrained_model)
    model.to(device)

    # Load your dataset (this is a placeholder, adapt to your data format)
    # You should implement a custom dataset loader for your images and JSON labels
    dataset = load_dataset('imagefolder', data_dir=args.train_dir)
    val_dataset = load_dataset('imagefolder', data_dir=args.val_dir)
    datasets = DatasetDict({
        'train': dataset['train'],
        'validation': val_dataset['train']
    })

    # Define training arguments
    training_args = TrainingArguments(
        output_dir=args.output_dir,
        per_device_train_batch_size=args.batch_size,
        per_device_eval_batch_size=args.batch_size,
        num_train_epochs=args.epochs,
        evaluation_strategy="epoch",
        save_strategy="epoch",
        logging_dir=os.path.join(args.output_dir, 'logs'),
        logging_steps=10,
        save_total_limit=2,
        fp16=torch.cuda.is_available(),
    )

    # You may need to implement a custom Trainer for Donut
    trainer = Trainer(
        model=model,
        args=training_args,
        train_dataset=datasets['train'],
        eval_dataset=datasets['validation'],
    )

    print("Starting training...")
    trainer.train()
    print(f"Saving model to {args.output_dir}")
    model.save_pretrained(args.output_dir)

if __name__ == "__main__":
    main()
