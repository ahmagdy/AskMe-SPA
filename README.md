# AskMe-SPA
The idea of this site to enable you to tell people anonymously about any thing you think about him and enable the person you send message to see it and reply it if he want .

## Available Online:
https://askme-spa.azurewebsites.net

## Download
```
$ git clone https://github.com/Ahmad-Magdy/AskMe-SPA.git askMeProject
$ cd askMeProject
$ npm install 
$ dotnet restore
```

## How it works:
- Web API by using ASP.NET Core MVC.
- Frontend: **Angular4+**  and **Bootstrap 4** Alpha.
- Authentication Based on JWT and accepts both Authorization Header with Bearer Schema and access_token cookie with HttpOnly preference.
- Database: SqlServer by using **EFCore** ORM
- User able to signup with username, email, name, password, description, image fields.
- Message has content, reply, and isVisible which is let user choose to view this message on his/her profile or not.

## Video:
[![YouTube](http://i3.ytimg.com/vi/eqy_h-HoH7o/maxresdefault.jpg)](https://youtu.be/eqy_h-HoH7o "Video Descripes the App - Click to Watch!")

## ScreenShots:

![1](http://i.imgur.com/0bt8zv8.png)
![2](http://i.imgur.com/2WhfDWE.png)
<img src="http://i.imgur.com/6x3X5dH.png" width="75%"/>


## License:
[The MIT License](https://github.com/Ahmad-Magdy/AskMe-SPA/blob/master/LICENSE)
