import"./chunk-WF6MHVQD.js";import"./chunk-ZUAI7IJ5.js";import"./chunk-YPCCXZX6.js";import"./chunk-FRDTDMZX.js";import"./chunk-LQL43AHG.js";import"./chunk-ZXCPSW3B.js";import"./chunk-OBWA6DFB.js";import"./chunk-RLVYGCTC.js";import"./chunk-2AZPC3K3.js";import{a as ne}from"./chunk-YHE4N465.js";import{a as te}from"./chunk-QXA7VEUO.js";import{a as ie}from"./chunk-CEX33KBR.js";import{a as Z,b as ee,c as y}from"./chunk-ZEESFLET.js";import{a as x}from"./chunk-7ICDGKY7.js";import"./chunk-ENGL65RD.js";import{a as T}from"./chunk-W4N5Z7QK.js";import"./chunk-32FVIJUL.js";import{d as U,e as W,f as E,g as P,h as G}from"./chunk-2IXFTJ2H.js";import{a as Q,b as q}from"./chunk-UHSV32VQ.js";import{b as H,d as J,i as V,q as K}from"./chunk-5ASYMZAG.js";import{a as X,b as Y}from"./chunk-PHZ4YKFG.js";import"./chunk-T2ZIFUV5.js";import"./chunk-KENT5INO.js";import{e as L}from"./chunk-FHVUNOKG.js";import"./chunk-3OH2LOAZ.js";import{H as A,Ib as a,Jb as f,Kb as u,Mb as F,Nb as B,Ob as N,P as b,Q as S,Qb as I,Rb as O,Sa as r,Sb as $,Ta as m,Ub as R,Vb as z,ba as _,db as D,f as C,fb as p,kb as M,lb as w,nb as j,ob as k,pb as i,qb as n,rb as s,wb as h,xb as v}from"./chunk-LUMD5QXA.js";import"./chunk-AIZVJUQQ.js";var pe=t=>["/project",t,"scan"];function se(t,g){if(t&1&&(i(0,"tr")(1,"td",25),s(2,"input",26),n(),i(3,"td")(4,"a",27),s(5,"ng-icon",28),i(6,"span",29),a(7),n()()(),i(8,"td")(9,"div",30)(10,"p",31),a(11),n(),i(12,"p",32),a(13),n(),i(14,"p",33),a(15),n(),i(16,"p",34),a(17),n(),i(18,"p",35),a(19),n()()(),i(20,"td")(21,"div",36)(22,"span",37),a(23),R(24,"timeago"),n()()(),s(25,"td"),n()),t&2){let e=g.$implicit,l=v(2);r(4),p("routerLink",$(12,pe,e.id)),r(),p("name",l.sourceIcon(e.sourceType)),r(2),f(e.name),r(4),u(" ",e.severityCritical," "),r(2),u(" ",e.severityHigh," "),r(2),u(" ",e.severityMedium," "),r(2),u(" ",e.severityLow," "),r(2),u(" ",e.severityInfo," "),r(3),p("tooltip",e.createdAt),r(),f(z(24,10,e.createdAt))}}function ce(t,g){if(t&1&&(i(0,"tbody"),j(1,se,26,14,"tr",null,w),n()),t&2){let e=v();r(),k(e.response.items)}}function me(t,g){t&1&&s(0,"loading-table")}var re=(()=>{class t{projectService;router;route;loading=!1;sorts=[{value:x.Name,label:"Name"},{value:x.CreatedAt,label:"Created"},{value:x.UpdatedAt,label:"Updated"}];response={count:0,currentPage:1,pageCount:0,items:[],size:0};filter={Name:"",SortBy:x.CreatedAt,Size:20,Page:1,Desc:!0};constructor(e,l,o){this.projectService=e,this.router=l,this.route=o}ngOnInit(){this.route.queryParams.pipe(b(e=>(this.loading=!0,ee(e,this.filter),this.filter.SortBy||(this.filter.SortBy=x.CreatedAt),this.projectService.getProjects(this.filter).pipe(A(()=>{this.loading=!1})))),S(this.destroy$)).subscribe(e=>{this.response=e})}onSearchChange(){y(this.router,this.filter)}onOrderChange(){this.filter.Desc=!this.filter.Desc,y(this.router,this.filter)}onSortChange(e){this.filter.SortBy=e,y(this.router,this.filter)}ngOnDestroy(){this.destroy$.next(null),this.destroy$.complete()}onChangePage(e){this.filter.Page=e,y(this.router,this.filter)}sourceIcon(e){return e?e.toString().toLowerCase():""}destroy$=new C;static \u0275fac=function(l){return new(l||t)(m(T),m(E),m(U))};static \u0275cmp=_({type:t,selectors:[["app-list"]],standalone:!0,features:[I],decls:37,vars:10,consts:[[1,"mx-auto","px-2","lg:px-6","flex","flex-col","pt-6","text-sm"],[1,"font-semibold","text-foreground","mb-2","ml-1","text-xl"],[1,"flex","min-w-full","flex-col","rounded-xl","border","border-border","bg-background"],[1,"flex","flex-col","py-3","px-5"],[1,"flex","flex-row","gap-2","w-full"],[1,"flex","w-full","md:max-w-80"],[1,"relative","text-muted-foreground","w-full"],[1,"absolute","left-2.5","top-2.5"],["name","heroMagnifyingGlass"],["placeholder","Search...","type","text",1,"py-2","pl-8","pr-2","w-full",3,"ngModelChange","keyup.enter","ngModel"],[1,"flex","flex-row","space-x-2","items-center"],[3,"selectedChange","options","selected"],["dropdown-label","",1,"font-semibold"],["size","16",1,"cursor-pointer",3,"click","name"],[1,"mt-3","px-1","py-1","whitespace-nowrap"],[1,"font-bold"],[1,"overflow-x-auto"],[1,"table","w-full","table-auto","border-collapse","border-0","text-left","align-middle","leading-5","text-muted-foreground"],[1,"border-t","border-border","text-xs","text-muted-foreground"],[1,"w-[50px]","min-w-[50px]","text-center"],["data-datatable-check","true","type","checkbox",1,"checkbox","checkbox-sm"],[1,"min-w-[300px]"],[1,"min-w-[180px]"],[1,"w-[60px]"],[1,"sticky","bottom-0","z-10",3,"pageChange","currentPage","totalPage"],[1,"text-center"],["data-datatable-row-check","true","type","checkbox","value","28",1,"checkbox","checkbox-sm"],[1,"flex","flex-row","items-center","gap-2",3,"routerLink"],[3,"name"],[1,"w-full"],[1,"mr-4","font-mono","text-xs","rounded-md","overflow-hidden","gap-1","shadow-sm","shadow-black/20","h-[28px]","flex","items-stretch"],[1,"flex-1","text-center","bg-rose-600/20","text-rose-500","flex","items-center","justify-center","opacity-100"],[1,"flex-1","text-center","bg-orange-600/20","text-orange-500","flex","items-center","justify-center","opacity-100"],[1,"flex-1","text-center","bg-yellow-600/20","text-yellow-500","flex","items-center","justify-center","opacity-100"],[1,"flex-1","text-center","bg-indigo-500/20","text-indigo-400","flex","items-center","justify-center","opacity-100"],[1,"flex-1","text-center","bg-green-500/20","text-green-400","flex","items-center","justify-center","opacity-100"],[1,"font-semibold","text-[#00c869]"],[3,"tooltip"]],template:function(l,o){l&1&&(i(0,"div",0)(1,"div",1),a(2,"Projects"),n(),i(3,"div",2)(4,"div",3)(5,"div",4)(6,"div",5)(7,"label",6)(8,"div",7),s(9,"ng-icon",8),n(),i(10,"input",9),N("ngModelChange",function(d){return B(o.filter.Name,d)||(o.filter.Name=d),d}),h("keyup.enter",function(){return o.onSearchChange()}),n()()(),i(11,"div",10)(12,"dropdown",11),h("selectedChange",function(d){return o.onSortChange(d)}),i(13,"span",12),a(14,"Sort By"),n()(),i(15,"ng-icon",13),h("click",function(){return o.onOrderChange()}),n()()(),i(16,"div",14),a(17),i(18,"span",15),a(19),n(),a(20," results"),n()(),i(21,"div",16)(22,"table",17)(23,"thead",18)(24,"tr")(25,"th",19),s(26,"input",20),n(),i(27,"th",21),a(28,"PROJECT"),n(),i(29,"th",22),a(30,"FINDING"),n(),i(31,"th",22),a(32,"CREATED AT"),n(),s(33,"th",23),n()(),D(34,ce,3,0,"tbody"),n()(),D(35,me,1,0,"loading-table"),i(36,"pagination",24),h("pageChange",function(d){return o.onChangePage(d)}),n()()()),l&2&&(r(10),F("ngModel",o.filter.Name),r(2),p("options",o.sorts)("selected",o.filter.SortBy),r(3),p("name",o.filter.Desc?"desc":"asc"),r(2),u("Displaying ",o.response.items==null?null:o.response.items.length," in "),r(2),f(o.response.count),r(15),M(o.loading?-1:34),r(),M(o.loading?35:-1),r(),p("currentPage",o.response.currentPage)("totalPage",o.response.pageCount))},dependencies:[q,L,K,H,J,V,Y,X,P,Q,te,ie]})}return t})();var de=()=>({exact:!1});function ue(t,g){if(t&1&&(i(0,"a",2)(1,"div",4)(2,"div",5),s(3,"ng-icon",6),i(4,"span",7),a(5),n()()()()),t&2){let e=g.$implicit,l=v();p("routerLink",l.routerLink(e.route)),r(2),p("routerLinkActiveOptions",O(4,de)),r(),p("name",e.icon),r(2),f(e.label)}}var oe=(()=>{class t{router;store;projectService;navItems=[{label:"Overview",route:"scan",icon:"scan",count:1},{label:"Finding",route:"finding",icon:"bug",count:123},{label:"Dependency",route:"dependency",icon:"inventory",count:53},{label:"Setting",route:"setting",icon:"setting"}];constructor(e,l,o){this.router=e,this.store=l,this.projectService=o,Z("projectId").pipe(b(c=>(this.store.projectId.set(c),this.projectService.getProjectInfo({projectId:c}))),S(this.destroy$)).subscribe(c=>{this.store.project.set(c)}),this.regexBaseUrl.test(this.router.url)&&this.router.navigate(["/project",this.store.projectId(),"scan"]).then()}ngOnDestroy(){this.destroy$.next(null),this.destroy$.complete()}ngOnInit(){}routerLink(e){return`/project/${this.store.projectId()}/${e}`}destroy$=new C;regexBaseUrl=new RegExp("^\\/project\\/[^\\/]+$");static \u0275fac=function(l){return new(l||t)(m(E),m(ne),m(T))};static \u0275cmp=_({type:t,selectors:[["app-project"]],standalone:!0,features:[I],decls:6,vars:0,consts:[[1,"bg-background","z-10","px-4"],[1,"flex","flex-wrap","items-center"],[1,"cursor-pointer","mr-3",3,"routerLink"],[1,"mx-auto","px-2","lg:px-6","flex","flex-col"],[1,"flex","items-center","mx-1"],["routerLinkActive","border-b-2 border-b-primary text-primary",1,"flex","flex-row","space-x-2","items-center","px-2","py-2","text-muted-foreground","text-xs","font-semibold",3,"routerLinkActiveOptions"],["size","16",1,"",3,"name"],[1,"tracking-wide","px-2","py-1","focus:outline-none","group-hover:text-foreground"]],template:function(l,o){l&1&&(i(0,"div",0)(1,"ul",1),j(2,ue,6,5,"a",2,w),n()(),i(4,"div",3),s(5,"router-outlet"),n()),l&2&&(r(2),k(o.navItems))},dependencies:[L,G,W,P]})}return t})();var Oe=[{path:"",component:re},{path:":projectId",component:oe,children:[{path:"scan",loadComponent:()=>import("./chunk-ADNU7BOY.js").then(t=>t.ScanComponent)},{path:"finding",loadComponent:()=>import("./chunk-ULWNV7U6.js").then(t=>t.FindingComponent)},{path:"dependency",loadComponent:()=>import("./chunk-7CV4PQWF.js").then(t=>t.DependencyComponent)},{path:"setting",loadChildren:()=>import("./chunk-Y4KMN44M.js").then(t=>t.routes)}]}];export{Oe as routes};
