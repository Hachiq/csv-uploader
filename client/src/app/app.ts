import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ContactService } from '../core/services/contact-service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected title = 'client';

  public contactService = inject(ContactService);

  constructor() {
    this.getContacts();
  }

  private getContacts() {
    this.contactService.getAll().subscribe(contacts => {
      console.log(contacts);
    });
  }
}
