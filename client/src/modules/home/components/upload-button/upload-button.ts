import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-upload-button',
  imports: [],
  templateUrl: './upload-button.html',
  styleUrl: './upload-button.scss'
})
export class UploadButton {
  selectedFile: File | null = null;

  @Output() fileUploaded = new EventEmitter<File>();

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  onUpload() {
    if (!this.selectedFile) {
      alert('Please select a file first.');
      return;
    }

    this.fileUploaded.emit(this.selectedFile);
    this.selectedFile = null;
  }
}
