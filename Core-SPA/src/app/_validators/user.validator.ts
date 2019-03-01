import { AbstractControl, ValidatorFn, FormGroup, Validators } from '@angular/forms';
import { AsyncValidatorFn, ValidationErrors } from '@angular/forms/src/directives/validators';


export class UserValidators {

    // static isUserNameTaken(service: AuthService): AsyncValidatorFn {
    //     return (c: AbstractControl): Promise<ValidationErrors> | Observable<ValidationErrors> => {            
    //         return Observable
    //             .timer(800)
    //             .switchMap(() => {
    //                 return service
    //                     .checkName(c.value)
    //                     .map((res) => {
    //                         return (res ? null : { 'isUserNameTaken': true });
    //                     });
    //             });
    //     };
    // }

    static matchPasswords(): ValidatorFn {
        return (c: AbstractControl): { [key: string]: boolean } | null => {
            if (c.value !== undefined && c.get('password').value !== c.get('confirmPassword').value) {
                c.get('confirmPassword').setErrors({ matchPasswords : true });
                return { matchPasswords : true };
            }
            c.get('confirmPassword').setErrors(null);
            return null;
        };
    }

    static emailOrEmpty(): ValidatorFn {
        return (c: AbstractControl): { [key: string]: boolean } | null => {
            if (!c.value) {
                return null;
            }

            return Validators.email(c);
        };
    }
}
