import{a as y}from"./chunk-FRDTDMZX.js";import{b as C}from"./chunk-ZEESFLET.js";import{a as g}from"./chunk-ENGL65RD.js";import{d as v,g as h}from"./chunk-2IXFTJ2H.js";import{r as b}from"./chunk-5ASYMZAG.js";import"./chunk-T2ZIFUV5.js";import"./chunk-KENT5INO.js";import"./chunk-FHVUNOKG.js";import"./chunk-3OH2LOAZ.js";import{H as l,Ib as n,Jb as f,Qb as x,Sa as m,Ta as s,ba as c,db as d,kb as p,pb as i,qb as o,xb as u}from"./chunk-LUMD5QXA.js";import"./chunk-AIZVJUQQ.js";function S(t,E){if(t&1&&(i(0,"p",4),n(1),o()),t&2){let r=u();m(),f(r.result.error)}}function _(t,E){t&1&&(i(0,"p",5),n(1,"Your account has been activated!"),o())}var M=(()=>{class t{authService;route;body={token:"",username:""};result={};loading=!1;constructor(r,e){this.authService=r,this.route=e,this.loading=!0,C(this.route.snapshot.queryParams,this.body),this.authService.confirmEmail({body:this.body}).pipe(l(()=>this.loading=!1)).subscribe(a=>{this.result=a})}static \u0275fac=function(e){return new(e||t)(s(y),s(v))};static \u0275cmp=c({type:t,selectors:[["app-confirm-email"]],standalone:!0,features:[x],decls:11,vars:1,consts:[[1,"my-10","space-y-6"],[1,"text-center"],[1,"mb-1","text-3xl","font-semibold","text-foreground"],[1,"text-primary"],[1,"text-sm","text-red-500"],[1,"text-sm","text-muted-foreground"],[1,"flex","justify-between","gap-2"],["app-button","","type","primary","routerLink","/auth/login",1,"w-full","font-semibold"]],template:function(e,a){e&1&&(i(0,"div",0)(1,"div",1)(2,"h2",2),n(3,"Active Account"),i(4,"span",3),n(5,"?"),o()(),d(6,S,2,1,"p",4)(7,_,2,0,"p",5),o(),i(8,"div",6)(9,"button",7),n(10,"Login Now"),o()()()),e&2&&(m(6),p(a.result.succeeded?7:6))},dependencies:[g,b,h]})}return t})();export{M as ConfirmEmailComponent};
