import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { FileService } from '../_services/file.service';

@Injectable()
export class ValidationMessagesResolver implements Resolve<JSON> {

    constructor(private fileService: FileService, private router: Router,
                private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<JSON> {
        return this.fileService.readJson('../../assets/validation-errors.json').pipe(
            catchError(error => {
                this.alertify.error(error);
                // this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}