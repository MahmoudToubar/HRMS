import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AccountService } from '../../core/services/account.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {

  constructor(
    public accountService: AccountService,
    private router: Router
  ) {}

  logout() {
    this.accountService.logout();
    this.router.navigate(['/login']);
  }

}
