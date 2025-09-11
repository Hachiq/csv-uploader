import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Contact } from '../../../../core/interfaces/contact';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-table',
  imports: [CommonModule, FormsModule],
  templateUrl: './table.html',
  styleUrl: './table.scss'
})
export class Table {
  @Input() contacts: Contact[] = [];
  displayedContacts: Contact[] = [];

  searchTerm: string = '';
  sortColumn?: keyof Contact;
  sortDirection: 'asc' | 'desc' = 'asc';

  @Output() onContactDelete = new EventEmitter<string>();

  ngOnChanges() {
    this.applyFiltersAndSorting();
  }

  onSearchChange() {
    this.applyFiltersAndSorting();
  }

  deleteContact(id: string) {
    this.onContactDelete.emit(id);
  }

  sortBy(column: keyof Contact) {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }

    this.applyFiltersAndSorting();
  }

  private applyFiltersAndSorting() {
    let result = [...this.contacts];

    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      result = result.filter(contact =>
        contact.name.toLowerCase().includes(term) ||
        contact.phone.toLowerCase().includes(term) ||
        contact.salary.toString().includes(term) ||
        (contact.isMarried ? 'yes' : 'no').includes(term)
      );
    }

    if (this.sortColumn) {
      const column = this.sortColumn;
      result.sort((a, b) => {
        const valueA = a[column] as any;
        const valueB = b[column] as any;

        if (valueA == null || valueB == null) return 0;

        if (typeof valueA === 'string' && typeof valueB === 'string') {
          return this.sortDirection === 'asc'
            ? valueA.localeCompare(valueB)
            : valueB.localeCompare(valueA);
        }

        return this.sortDirection === 'asc'
          ? valueA > valueB ? 1 : -1
          : valueA < valueB ? 1 : -1;
      });
    }

    this.displayedContacts = result;
  }
}
