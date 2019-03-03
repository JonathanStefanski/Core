import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Pagination } from '../_models/Pagination';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MemberListResolver implements Resolve<User[]> {
    constructor(private userService: UserService, private router: Router,
                private alertify: AlertifyService, private authService: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
        const id = this.authService.decodedToken.nameid;
        const pagination: Pagination = JSON.parse(localStorage.getItem(`matchesPagination_${id}`));
        const pageNumber = pagination == null ? 1 : pagination.currentPage;
        const pageSize = pagination == null ? 5 : pagination.itemsPerPage;

        return this.userService.getUsers(pageNumber, pageSize).pipe(
            catchError(error => {
                this.alertify.error(error);
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
