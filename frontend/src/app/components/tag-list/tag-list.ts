import { Component, input } from '@angular/core';

@Component({
  selector: 'app-tag-list',
  standalone: true,
  imports: [],
  templateUrl: './tag-list.html',
  styleUrl: './tag-list.css',
})
export class TagList {
  tags = input<string[] | undefined>([]);
}
