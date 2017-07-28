import { Router } from '@angular/router';
import { IMessage } from './../../../interfaces/IMessage';
import { Observable } from 'rxjs/Observable';
import { Component, OnInit } from '@angular/core';
import { CommService } from './../../../services/comm.service';
import { SnotifyService } from 'ng-snotify';


@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: IMessage[] = <IMessage[]>[];
  constructor(private service: CommService, private toast: SnotifyService,
    private router: Router) { }

  ngOnInit() {
    this.service.allMessages().subscribe(res => this.messages = res.reverse());
  }

  reverseVisible(id: number, visible: boolean) {
    this.service.changeVisibility(id)
      .subscribe((res) => {
        if (!res) {
          this.toast.info('Done');
          this.messages.find(m => m.id === id).isVisible = !visible;
        }
      });
  }

  deleteMessage(id) {
    this.toast.confirm('Do you realy need to delete this message', 'Are you sure', {
      buttons: [
        {
          text: 'Yes', action: (toastId) => {
            this.toast.remove(toastId);
            this.service.deleteMessage(id)
              .subscribe((res) => {
                if (!res) {
                  this.toast.simple('Deleted');
                  this.messages = this.messages.filter(m => m.id !== id);
                }
              });

          }
        },
        { text: 'No', action: (toastId) => { this.toast.remove(toastId); } },
      ],
      timeout: 5000,
      closeOnClick: false
    });

  }


  navigateToReply(message: IMessage) {
    this.service.passMessage(message);
    this.router.navigate(['/dashboard/messages/reply', message.id]);
  }

}
