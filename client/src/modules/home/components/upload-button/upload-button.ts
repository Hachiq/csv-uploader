import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-upload-button',
  imports: [],
  templateUrl: './upload-button.html',
  styleUrl: './upload-button.scss'
})
export class UploadButton {
  @Output() fileUploaded = new EventEmitter<File>();

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.fileUploaded.emit(file);
    }
  }
}
