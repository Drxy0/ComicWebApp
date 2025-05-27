import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { ProfileComponent } from './user/profile/profile.component';
import { isAuthenticatedGuard } from './shared/auth.guard';
import { ComicComponent } from './comic-series/comic/comic.component';

export const routes: Routes = [
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'register',
        component: RegisterComponent
    },
    {
        path: 'profile',
        canActivate: [isAuthenticatedGuard()],
        component: ProfileComponent
    },
    {
        path: 'comic/:id',
        component: ComicComponent
    }
];
