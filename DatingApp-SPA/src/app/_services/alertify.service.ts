import { Injectable } from '@angular/core';
// import 'node_modules/alertifyjs/build/alertify.min.js';
declare let alertify: any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

constructor() { }

confirm(message: string , okcallback: () => any) {

  alertify.confirm(message, function(e) {
                if (e) {
                okcallback();
                } else {

                }
                  });
}

success(message: string) {
  alertify.success(message);

}

error(message: string) {
  alertify.error(message);
}
warning(message: string) {
  alertify.warning(message);
}
message(message: string) {
  alertify.message(message);

}

}
