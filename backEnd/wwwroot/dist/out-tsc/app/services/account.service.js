var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
let AccountService = class AccountService {
    constructor(_http) {
        this._http = _http;
        this.host = "http://paintstorerest.azurewebsites.net/";
    }
    selectUserById(id) {
        return this._http.get(this.host + "api/Posts/Users/" + id);
    }
    getUserToken(data) {
        // let data = {
        //   email: name,
        //   password: password
        // };
        return this._http.post(this.host + "api/SignIn/In", data);
    }
    registerUser(data) {
        return this._http.post(this.host + "api/Users/AddUser", data);
    }
    logoutUser(data, id, token) {
        let headers = new HttpHeaders();
        headers = headers.append("Authorization", "Basic " + btoa("" + id + ":" + token));
        headers = headers.append("Content-Type", "application/json");
        return this._http.post(this.host + "api/SignIn/Out", data, {
            headers: headers
        });
    }
};
AccountService = __decorate([
    Injectable({
        providedIn: "root"
    }),
    __metadata("design:paramtypes", [HttpClient])
], AccountService);
export { AccountService };
//# sourceMappingURL=account.service.js.map