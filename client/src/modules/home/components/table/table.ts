import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Contact } from '../../../../core/interfaces/contact';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-table',
  imports: [CommonModule],
  templateUrl: './table.html',
  styleUrl: './table.scss'
})
export class Table {
  @Input() contacts: Contact[] = [];

  @Output() onContactDelete = new EventEmitter<string>();

  deleteContact(id: string) {
    this.onContactDelete.emit(id);
  }
}
