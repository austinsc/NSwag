﻿//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v5.5.6093.36414 (http://NSwag.org)
// </auto-generated>
//----------------------

import 'rxjs/Rx'; 
import {Observable} from 'rxjs/Observable';
import {Injectable, Inject, Optional, OpaqueToken} from '@angular/core';
import {Http, Headers, Response, RequestOptionsArgs} from '@angular/http';

export const API_BASE_URL = new OpaqueToken('API_BASE_URL');
export const JSON_PARSE_REVIVER = new OpaqueToken('JSON_PARSE_REVIVER');

@Injectable()
export class PersonsClient {
    private http: Http = null; 
    private baseUrl: string = undefined; 
    private jsonParseReviver: (key: string, value: any) => any = undefined; 

    constructor(@Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string, @Optional() @Inject(JSON_PARSE_REVIVER) jsonParseReviver?: (key: string, value: any) => any) {
        this.http = http; 
        this.baseUrl = baseUrl ? baseUrl : "http://localhost:13452"; 
        this.jsonParseReviver = jsonParseReviver;
    }

    getAll(): Observable<Person[]> {
        var url = this.baseUrl + "/api/Persons"; 

        var content = "";
        
        return this.http.request(url, {
            body: content,
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8"
            })
        }).map((response) => {
            return this.processGetAll(response);
        });
    }

    private processGetAll(response: Response) {
        var data = response.text();
        var status = response.status.toString(); 

        if (status === "200") {
            var result200: Person[] = null; 
            var resultData200 = data === "" ? null : JSON.parse(data, this.jsonParseReviver);
            if (resultData200 && resultData200.constructor === Array) {
                result200 = [];
                for (let item of resultData200)
                    result200.push(Person.fromJS(item));
            }
            return result200; 
        }
        else
        {
            throw "error_no_callback_for_the_received_http_status"; 
        }
    }

    add(person: Person): Observable<void> {
        var url = this.baseUrl + "/api/Persons"; 

        var content = JSON.stringify(person ? person.toJS() : null);
        
        return this.http.request(url, {
            body: content,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8"
            })
        }).map((response) => {
            return this.processAdd(response);
        });
    }

    private processAdd(response: Response) {
        var data = response.text();
        var status = response.status.toString(); 

        if (status === "204") {
        }
        else
        {
            throw "error_no_callback_for_the_received_http_status"; 
        }
    }

    get(id: string): Observable<Person> {
        var url = this.baseUrl + "/api/Persons/{id}"; 

        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url = url.replace("{id}", encodeURIComponent("" + id)); 

        var content = "";
        
        return this.http.request(url, {
            body: content,
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8"
            })
        }).map((response) => {
            return this.processGet(response);
        });
    }

    private processGet(response: Response) {
        var data = response.text();
        var status = response.status.toString(); 

        if (status === "200") {
            var result200: Person = null; 
            var resultData200 = data === "" ? null : JSON.parse(data, this.jsonParseReviver);
            result200 = resultData200 ? Person.fromJS(resultData200) : new Person();
            return result200; 
        }
        else
        if (status === "500") {
            var result500: PersonNotFoundException = null; 
            var resultData500 = data === "" ? null : JSON.parse(data, this.jsonParseReviver);
            result500 = resultData500 ? PersonNotFoundException.fromJS(resultData500) : new PersonNotFoundException();
            throw result500; 
        }
        else
        {
            throw "error_no_callback_for_the_received_http_status"; 
        }
    }

    delete(id: string): Observable<void> {
        var url = this.baseUrl + "/api/Persons/{id}"; 

        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url = url.replace("{id}", encodeURIComponent("" + id)); 

        var content = "";
        
        return this.http.request(url, {
            body: content,
            method: "delete",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8"
            })
        }).map((response) => {
            return this.processDelete(response);
        });
    }

    private processDelete(response: Response) {
        var data = response.text();
        var status = response.status.toString(); 

        if (status === "204") {
        }
        else
        {
            throw "error_no_callback_for_the_received_http_status"; 
        }
    }

    /**
     * Gets the name of a person.
     * @id The person ID.
     * @return The person's name.
     */
    getName(id: string): Observable<string> {
        var url = this.baseUrl + "/api/Persons/{id}/Name"; 

        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url = url.replace("{id}", encodeURIComponent("" + id)); 

        var content = "";
        
        return this.http.request(url, {
            body: content,
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8"
            })
        }).map((response) => {
            return this.processGetName(response);
        });
    }

    private processGetName(response: Response) {
        var data = response.text();
        var status = response.status.toString(); 

        if (status === "200") {
            var result200: string = null; 
            var resultData200 = data === "" ? null : JSON.parse(data, this.jsonParseReviver);
            result200 = resultData200 !== undefined ? resultData200 : null;
            return result200; 
        }
        else
        if (status === "500") {
            var result500: PersonNotFoundException = null; 
            var resultData500 = data === "" ? null : JSON.parse(data, this.jsonParseReviver);
            result500 = resultData500 ? PersonNotFoundException.fromJS(resultData500) : new PersonNotFoundException();
            throw result500; 
        }
        else
        {
            throw "error_no_callback_for_the_received_http_status"; 
        }
    }
}

@Injectable()
export class GeoClient {
    private http: Http = null; 
    private baseUrl: string = undefined; 
    private jsonParseReviver: (key: string, value: any) => any = undefined; 

    constructor(@Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string, @Optional() @Inject(JSON_PARSE_REVIVER) jsonParseReviver?: (key: string, value: any) => any) {
        this.http = http; 
        this.baseUrl = baseUrl ? baseUrl : "http://localhost:13452"; 
        this.jsonParseReviver = jsonParseReviver;
    }

    fromBodyTest(location: GeoPoint): Observable<void> {
        var url = this.baseUrl + "/api/Geo/FromBodyTest"; 

        var content = JSON.stringify(location ? location.toJS() : null);
        
        return this.http.request(url, {
            body: content,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8"
            })
        }).map((response) => {
            return this.processFromBodyTest(response);
        });
    }

    private processFromBodyTest(response: Response) {
        var data = response.text();
        var status = response.status.toString(); 

        if (status === "204") {
        }
        else
        {
            throw "error_no_callback_for_the_received_http_status"; 
        }
    }

    fromUriTest(latitude: number, longitude: number): Observable<void> {
        var url = this.baseUrl + "/api/Geo/FromUriTest?"; 

        if (latitude === null)
            throw new Error("The parameter 'latitude' cannot be null.");
        else if (latitude !== undefined)
            url += "Latitude=" + encodeURIComponent("" + latitude) + "&"; 
        if (longitude === null)
            throw new Error("The parameter 'longitude' cannot be null.");
        else if (longitude !== undefined)
            url += "Longitude=" + encodeURIComponent("" + longitude) + "&"; 

        var content = "";
        
        return this.http.request(url, {
            body: content,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8"
            })
        }).map((response) => {
            return this.processFromUriTest(response);
        });
    }

    private processFromUriTest(response: Response) {
        var data = response.text();
        var status = response.status.toString(); 

        if (status === "204") {
        }
        else
        {
            throw "error_no_callback_for_the_received_http_status"; 
        }
    }

    addPolygon(points: GeoPoint[]): Observable<void> {
        var url = this.baseUrl + "/api/Geo/AddPolygon"; 

        var contentData: any = [];
        if (points) {
            for (let item of points)
                contentData.push(item.toJS());
        }
        var content = JSON.stringify(points ? contentData : null);
        
        return this.http.request(url, {
            body: content,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8"
            })
        }).map((response) => {
            return this.processAddPolygon(response);
        });
    }

    private processAddPolygon(response: Response) {
        var data = response.text();
        var status = response.status.toString(); 

        if (status === "204") {
        }
        else
        {
            throw "error_no_callback_for_the_received_http_status"; 
        }
    }
}

export class Person { 
    id: string; 
    /** Gets or sets the first name. */
    firstName: string; 
    /** Gets or sets the last name. */
    lastName: string; 
    gender: Gender; 
    dateOfBirth: Date; 
    weight: number; 
    height: number; 
    age: number; 
    address: Address = new Address(); 
    children: Person[] = []; 
    skills: { [key: string] : SkillLevelAsInteger; }; 
    protected discriminator: string;

    constructor(data?: any) {
        this.discriminator = "Person";
        if (data !== undefined) {
            this.id = data["Id"] !== undefined ? data["Id"] : null;
            this.firstName = data["FirstName"] !== undefined ? data["FirstName"] : null;
            this.lastName = data["LastName"] !== undefined ? data["LastName"] : null;
            this.gender = data["Gender"] !== undefined ? data["Gender"] : null;
            this.dateOfBirth = data["DateOfBirth"] ? new Date(data["DateOfBirth"].toString()) : null;
            this.weight = data["Weight"] !== undefined ? data["Weight"] : null;
            this.height = data["Height"] !== undefined ? data["Height"] : null;
            this.age = data["Age"] !== undefined ? data["Age"] : null;
            this.address = data["Address"] ? Address.fromJS(data["Address"]) : new Address();
            if (data["Children"] && data["Children"].constructor === Array) {
                this.children = [];
                for (let item of data["Children"])
                    this.children.push(Person.fromJS(item));
            }
            if (data["Skills"]) {
                this.skills = {};
                for (let key in data["Skills"]) {
                    if (data["Skills"].hasOwnProperty(key))
                        this.skills[key] = data["Skills"][key] !== undefined ? data["Skills"][key] : null;
                }
            }
            this.discriminator = data["discriminator"] !== undefined ? data["discriminator"] : null;
        }
    }

    static fromJS(data: any): Person {
        if (data["discriminator"] === "Teacher")
            return new Teacher(data);
        return new Person(data);
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data["Id"] = this.id !== undefined ? this.id : null;
        data["FirstName"] = this.firstName !== undefined ? this.firstName : null;
        data["LastName"] = this.lastName !== undefined ? this.lastName : null;
        data["Gender"] = this.gender !== undefined ? this.gender : null;
        data["DateOfBirth"] = this.dateOfBirth ? this.dateOfBirth.toISOString() : null;
        data["Weight"] = this.weight !== undefined ? this.weight : null;
        data["Height"] = this.height !== undefined ? this.height : null;
        data["Age"] = this.age !== undefined ? this.age : null;
        data["Address"] = this.address ? this.address.toJS() : null;
        if (this.children && this.children.constructor === Array) {
            data["Children"] = [];
            for (let item of this.children)
                data["Children"].push(item.toJS());
        }
        if (this.skills) {
            data["Skills"] = {};
            for (let key in this.skills) {
                if (this.skills.hasOwnProperty(key))
                    data["Skills"][key] = this.skills[key] !== undefined ? this.skills[key] : null;
            }
        }
        data["discriminator"] = this.discriminator !== undefined ? this.discriminator : null;
        return data; 
    }

    toJSON() {
        return JSON.stringify(this.toJS());
    }

    clone() {
        var json = this.toJSON();
        return new Person(JSON.parse(json));
    }
}

export class Teacher extends Person { 
    course: string;

    constructor(data?: any) {
        super(data);
        this.discriminator = "Teacher";
        if (data !== undefined) {
            this.course = data["Course"] !== undefined ? data["Course"] : null;
        }
    }

    static fromJS(data: any): Teacher {
        return new Teacher(data);
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data["Course"] = this.course !== undefined ? this.course : null;
        super.toJS(data);
        return data; 
    }

    toJSON() {
        return JSON.stringify(this.toJS());
    }

    clone() {
        var json = this.toJSON();
        return new Teacher(JSON.parse(json));
    }
}

export enum Gender {
    Male = <any>"Male", 
    Female = <any>"Female", 
}

export class Address { 
    isPrimary: boolean; 
    city: string;

    constructor(data?: any) {
        if (data !== undefined) {
            this.isPrimary = data["IsPrimary"] !== undefined ? data["IsPrimary"] : null;
            this.city = data["City"] !== undefined ? data["City"] : null;
        }
    }

    static fromJS(data: any): Address {
        return new Address(data);
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data["IsPrimary"] = this.isPrimary !== undefined ? this.isPrimary : null;
        data["City"] = this.city !== undefined ? this.city : null;
        return data; 
    }

    toJSON() {
        return JSON.stringify(this.toJS());
    }

    clone() {
        var json = this.toJSON();
        return new Address(JSON.parse(json));
    }
}

export enum SkillLevelAsInteger {
    Low = 0, 
    Medium = 1, 
    Height = 2, 
}

export class Exception { 
    message: string; 
    innerException: Exception = new Exception(); 
    stackTrace: string; 
    source: string;

    constructor(data?: any) {
        if (data !== undefined) {
            this.message = data["Message"] !== undefined ? data["Message"] : null;
            this.innerException = data["InnerException"] ? Exception.fromJS(data["InnerException"]) : new Exception();
            this.stackTrace = data["StackTrace"] !== undefined ? data["StackTrace"] : null;
            this.source = data["Source"] !== undefined ? data["Source"] : null;
        }
    }

    static fromJS(data: any): Exception {
        return new Exception(data);
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data["Message"] = this.message !== undefined ? this.message : null;
        data["InnerException"] = this.innerException ? this.innerException.toJS() : null;
        data["StackTrace"] = this.stackTrace !== undefined ? this.stackTrace : null;
        data["Source"] = this.source !== undefined ? this.source : null;
        return data; 
    }

    toJSON() {
        return JSON.stringify(this.toJS());
    }

    clone() {
        var json = this.toJSON();
        return new Exception(JSON.parse(json));
    }
}

export class PersonNotFoundException extends Exception { 
    id: string;

    constructor(data?: any) {
        super(data);
        if (data !== undefined) {
            this.id = data["id"] !== undefined ? data["id"] : null;
        }
    }

    static fromJS(data: any): PersonNotFoundException {
        return new PersonNotFoundException(data);
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data["id"] = this.id !== undefined ? this.id : null;
        super.toJS(data);
        return data; 
    }

    toJSON() {
        return JSON.stringify(this.toJS());
    }

    clone() {
        var json = this.toJSON();
        return new PersonNotFoundException(JSON.parse(json));
    }
}

export class GeoPoint { 
    latitude: number; 
    longitude: number;

    constructor(data?: any) {
        if (data !== undefined) {
            this.latitude = data["Latitude"] !== undefined ? data["Latitude"] : null;
            this.longitude = data["Longitude"] !== undefined ? data["Longitude"] : null;
        }
    }

    static fromJS(data: any): GeoPoint {
        return new GeoPoint(data);
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data["Latitude"] = this.latitude !== undefined ? this.latitude : null;
        data["Longitude"] = this.longitude !== undefined ? this.longitude : null;
        return data; 
    }

    toJSON() {
        return JSON.stringify(this.toJS());
    }

    clone() {
        var json = this.toJSON();
        return new GeoPoint(JSON.parse(json));
    }
}