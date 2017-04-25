import { Component } from '@angular/core';
import { AppService } from './app.service';
import { MoveModel } from './move.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  private title = 'app works!';
  private token: string;
  private moveInfo: MoveModel;
  private userName: string;
  private sides: Array<string> = ['black', 'white'];
  private selectedSide: string = this.sides[0];

  constructor(private appService: AppService) { }

  private login(): void {
    console.log(this.userName);
    this.appService.login(this.userName, this.selectedSide).then(token => {
      this.token = token;
      console.log('token:');
      console.log(token);
    });
  }

  private move(): void {
    this.appService.move(this.userName, this.token).then(move => {
      this.moveInfo = move;
      console.log(move);
    });
  }

  private resetGame(): void {
    this.appService.resetGame();
  }

  private onSelectionChange(side) {
    // clone the object for immutability
    this.selectedSide = side;
  }
}
