import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { CommService } from './../../../services/comm.service';
import { IMessage } from "../../../interfaces/IMessage";
import { SnotifyService } from "ng-snotify";


@Component({
  selector: 'app-message-reply',
  templateUrl: './message-reply.component.html',
  styleUrls: ['./message-reply.component.css']
})
export class MessageReplyComponent implements OnInit {
  reply: string;
  message: IMessage;
  constructor(private route: ActivatedRoute, private service: CommService, private toast: SnotifyService) { }

  ngOnInit() {
    this.service.messageToReply.subscribe(res => {
      this.message = res;
    });
    //   this.route.params.subscribe(par => {
    //     const id = par['id'];
    //     this.servce.getPreviousReply(id)
    //       .subscribe(res => {
    //         console.log(res);
    //         this.reply = res.reply;
    //         console.log(this.reply);
    //       });

    //   });

  }

  submit() {
    console.log(this.message);
   
    if (/^\s*$/.test(this.message.reply)) {
      this.toast.warning('Enter a Valid Reply', 'Wrong!!!', { timeout: 5000 });
    } else {
       this.service.sendReply(this.message);
    }
  }

}
