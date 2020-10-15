import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BusyService } from '../services/busy.service';
import { Injectable } from '@angular/core';
import { delay, finalize } from 'rxjs/operators';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
    constructor(private busyService: BusyService){}
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // 272-1 turn off spinner when posting order, because stripe's iframe.
        if (req.method === 'POST' && req.url.includes('orders')) {
          return next.handle(req);
        }

        // 286- don't show spinner when deleting basket.
        if (req.method === 'DELETE') {
          return next.handle(req);
        }

        if (!req.url.includes('emailexists')) {
            this.busyService.busy();
        }

        // 272-2
        // this.busyService.busy();

        return next.handle(req).pipe(
            // delay(1000),
            finalize(() => {
                this.busyService.idle();
            })
        );
    }

}
