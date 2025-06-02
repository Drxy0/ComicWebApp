import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { ProfileComponent } from './user/profile/profile.component';
import { ComicComponent } from './comic-series/comic/comic.component';
import { isAuthenticatedGuard } from './core/guards/auth.guard';
import { ReaderComponent } from './comic-series/reader/reader.component';

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
    },
    {
        path: 'comic/:chapterId/:pageNumber',
        component: ReaderComponent
    }
];
