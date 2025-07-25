import { Routes } from '@angular/router';
import { AuthComponent } from './auth/auth.component';
import { AuthGuard } from './guards/auth-guard';
import { GroupComponent } from './group/group.component';
import { UpdatemeComponent } from './updateme/updateme.component';
import { TasksComponent } from './tasks/tasks.component';

export const routes: Routes = [
  { canActivate: [AuthGuard], path: '', component: GroupComponent },
  { path: 'auth', component: AuthComponent },
  { path: 'updateme', component: UpdatemeComponent, canActivate: [AuthGuard] },
  { path: 'group/:guid', component: TasksComponent, canActivate: [AuthGuard] },
];
