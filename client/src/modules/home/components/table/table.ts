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
  sortedContacts: Contact[] = [];

  sortColumn: keyof Contact | '' = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  @Output() onContactDelete = new EventEmitter<string>();

  ngOnChanges() {
    this.sortedContacts = [...this.contacts];
  }

  deleteContact(id: string) {
    this.onContactDelete.emit(id);
  }

  sortBy(column: keyof Contact) {
    console.log(`Sorting by ${column}`);
    if (this.sortColumn === column) {
      // toggle direction
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }

    this.sortedContacts.sort((a, b) => {
      const valueA = a[column];
      const valueB = b[column];

      if (valueA == null || valueB == null) return 0;

      if (typeof valueA === 'string' && typeof valueB === 'string') {
        return this.sortDirection === 'asc'
          ? valueA.localeCompare(valueB)
          : valueB.localeCompare(valueA);
      }

      return this.sortDirection === 'asc'
        ? (valueA as any) > (valueB as any) ? 1 : -1
        : (valueA as any) < (valueB as any) ? 1 : -1;
    });
  }
}
