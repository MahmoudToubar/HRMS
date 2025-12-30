import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './confirm-dialog.component.html',
  styleUrl: './confirm-dialog.component.css'
})
export class ConfirmDialogComponent {

  @Input() title = 'Confirm';
  @Input() message = 'Are you sure?';
  @Input() show = false;

  @Output() confirm = new EventEmitter<boolean>();

  onConfirm(): void {
    this.confirm.emit(true);
    this.show = false;
  }

  onCancel(): void {
    this.confirm.emit(false);
    this.show = false;
  }

}
