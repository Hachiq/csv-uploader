import { Component, inject } from '@angular/core';
import { ContactService } from '../../core/services/contact-service';
import { UploadButton } from "./components/upload-button/upload-button";
import { Table } from "./components/table/table";
import { Contact } from '../../core/interfaces/contact';

@Component({
  selector: 'app-home',
  imports: [UploadButton, Table],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home {
  protected title = 'Home';

  contacts: Contact[] = [];

  public contactService = inject(ContactService);

  constructor() {
    this.getContacts();
  }

  private getContacts() {
    this.contactService.getAll().subscribe(c => this.contacts = c);
  }

  onFileUploaded(file: File) {
    this.contactService.upload(file).subscribe(() => this.getContacts());
  }

  deleteContact(id: string) {
    this.contactService.delete(id).subscribe(() => this.getContacts());
  }

  updateContact(event: { id: string, contact: Partial<Contact> }) {
    this.contactService.update(event.id, event.contact).subscribe(() => this.getContacts());
  }
}
