﻿import { Component } from '@angular/core';
import { Http, RequestOptions, Response, Headers } from '@angular/http'; // npm install @angular/http@latest
//import { Observable } from 'rxjs'; // npm install --save rxjs-compat
//import { map } from 'rxjs/operators';
import 'rxjs/add/operator/map' //npm install rxjs@6 rxjs-compat@6 --save
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(private _http : Http) { }
  public model: any = { }; //  get các giá trị trên form
  title = 'app';
  public email: string;

  public login() {
    var data = {
      UserName: this.model.username,
      Password: this.model.password
    }
    let headers = new Headers();
    // headers.append("Content-Type", "application/x-www-form-urlencoded");
    headers.append('Content-Type', 'application/json');
    // headers.append("Authorization", "Bearer " + this.user.access_token);
    let options = new RequestOptions({ headers: headers });

    this._http.post('https://localhost:44303/api/Account/login',
      JSON.stringify(data), options).map((response: Response) => {
        let result = response.json();
        var base64Url = result.token.split('.')[1]; // parse token gắn trên header response
        var base64 = base64Url.replace('-', '+').replace('_', '/');
        var user = JSON.parse(window.atob(base64));
        console.log(user);

        this.email = user.email;

      }).subscribe( data => {
        console.log(data);
      });
  }
}
