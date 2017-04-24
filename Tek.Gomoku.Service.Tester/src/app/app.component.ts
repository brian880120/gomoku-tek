import { Component } from '@angular/core';
import { AppService } from './app.service';
import { MoveModel } from './move.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  title = 'app works!';

  token: string;

  moveInfo: MoveModel;

  constructor(private appService: AppService) { }

  private login(): void {
    this.appService.login('hong.xu', 'P@ssw0rd!').then(token => {
      this.token = token;
      console.log('token:');
      console.log(token);
    });
  }

  private move(): void {
    this.appService.move('hong.xu', this.token).then(move => {
      this.moveInfo = move;
      console.log(move);
    });
  }
}
