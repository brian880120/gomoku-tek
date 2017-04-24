import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { CredentialModel } from './credential.model';
import { LoginResultModel } from './login.result.model';
import { MoveModel } from './move.model';

@Injectable()
export class AppService {

  private authWebApiUrl = 'http://localhost:5000/api/authentication/token';
  private gameWebApiUrl = "http://localhost:5000/api/gamemoves";

  constructor(private http: Http) { }

  public login(userName: string, password: string): Promise<string> {

    var credential = new CredentialModel(userName, password);

    return this.http.post(this.authWebApiUrl, credential)
      .toPromise()
      .then(response => {
        var result = response.json() as LoginResultModel;
        return result.token;
      });
  }

  public move(userName: string, token: string): Promise<MoveModel> {

    let move = new MoveModel();
    move.columnIndex = '1';
    move.rowIndex = '2';
    move.playerName = userName;

    let headers = new Headers();
    headers.append('Authorization', 'bearer ' + token);

    return this.http.post(this.gameWebApiUrl, move, { headers: headers })
      .toPromise()
      .then(response => {
        var result = response.json() as MoveModel;
        return result;
      });
  }
}
