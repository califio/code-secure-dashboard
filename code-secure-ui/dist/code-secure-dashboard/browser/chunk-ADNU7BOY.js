import{c as te,d as ie}from"./chunk-JLJF64GP.js";import"./chunk-4TJNVXQH.js";import{a as ae}from"./chunk-YHE4N465.js";import{e as le}from"./chunk-KJV7B77S.js";import"./chunk-QXA7VEUO.js";import{a as y,c as d}from"./chunk-ZSGDYSVW.js";import{a as re}from"./chunk-CEX33KBR.js";import{b as ne}from"./chunk-ZEESFLET.js";import"./chunk-7ICDGKY7.js";import"./chunk-ENGL65RD.js";import{a as oe}from"./chunk-W4N5Z7QK.js";import"./chunk-32FVIJUL.js";import{d as H,g as J}from"./chunk-2IXFTJ2H.js";import{a as K,b as V}from"./chunk-UHSV32VQ.js";import{q as W,r as X}from"./chunk-5ASYMZAG.js";import{a as Z,b as ee}from"./chunk-PHZ4YKFG.js";import"./chunk-T2ZIFUV5.js";import"./chunk-KENT5INO.js";import{e as I}from"./chunk-FHVUNOKG.js";import{i as Q,m as Y}from"./chunk-3OH2LOAZ.js";import{H as T,Ib as o,Jb as f,Kb as h,Oa as j,P as N,Q as R,Qb as w,Rb as O,Sa as r,Sb as _,Ta as b,Tb as k,Ub as E,Vb as P,Wb as G,ba as C,db as x,ea as $,f as B,fb as s,kb as g,nb as q,ob as z,pb as i,qb as t,rb as m,wb as U,xb as v}from"./chunk-LUMD5QXA.js";import"./chunk-AIZVJUQQ.js";function pe(e,p){if(e&1&&(i(0,"span",2),o(1),t()),e&2){let n=v();r(),f(n.status)}}var se=(()=>{class e{status=d.Queue;showLabel=!0;getStyle(n){return this.mStyle.get(n)}getIcon(n){return this.mIcon.get(n)}mStyle=new Map([[d.Queue,"text-orange-500"],[d.Running,"text-blue-500 animate-spin"],[d.Completed,"text-green-500"],[d.Error,"text-red-500"]]);mIcon=new Map([[d.Queue,"heroPauseCircle"],[d.Running,"spin"],[d.Completed,"heroCheckCircle"],[d.Error,"heroMinusCircle"]]);static \u0275fac=function(c){return new(c||e)};static \u0275cmp=C({type:e,selectors:[["scan-status"]],inputs:{status:"status",showLabel:"showLabel"},standalone:!0,features:[w],decls:3,vars:3,consts:[[1,"flex","flex-row","items-center","gap-2"],["size","20",3,"name","ngClass"],[1,"lowercase"]],template:function(c,a){c&1&&(i(0,"div",0),m(1,"ng-icon",1),x(2,pe,2,1,"span",2),t()),c&2&&(r(),s("name",a.getIcon(a.status))("ngClass",a.getStyle(a.status)),r(),g(a.showLabel?2:-1))},dependencies:[I,Q]})}return e})();var me=(()=>{class e{transform(n,c=20){return n.length<c?n:n.slice(0,c)+"..."}static \u0275fac=function(c){return new(c||e)};static \u0275pipe=$({name:"truncate",type:e,pure:!0,standalone:!0})}return e})();var de=(e,p)=>p.id,L=e=>["/project",e,"finding"],ue=()=>({status:"Open"}),fe=(e,p)=>({commitId:e,scanner:p}),xe=(e,p)=>({scanId:e,branch:p});function ge(e,p){e&1&&m(0,"ng-icon",8)}function ve(e,p){if(e&1&&(i(0,"a",9),o(1),t()),e&2){let n=v();s("routerLink",_(3,L,n.store.projectId()))("queryParams",O(5,ue)),r(),h(" ",n.statistic.openFinding," ")}}function he(e,p){e&1&&m(0,"loading-table",11),e&2&&s("row",5)}function Se(e,p){if(e&1&&(i(0,"div",30)(1,"div",31),o(2,"Static Application Security Testing (SAST)"),t(),i(3,"div",32),m(4,"div",33)(5,"finding-status-chart",34),t()(),i(6,"div",30)(7,"div",31),o(8,"Software Composition Analysis (SCA)"),t(),i(9,"div",32),m(10,"div",33)(11,"finding-status-chart",34),t()()),e&2){let n=v();r(4),s("severity",n.statistic.severitySast),r(),s("status",n.statistic.statusSast),r(5),s("severity",n.statistic.severitySca),r(),s("status",n.statistic.statusSca)}}function ye(e,p){if(e&1&&(i(0,"tr")(1,"td",35),m(2,"input",36),t(),i(3,"td")(4,"a",37)(5,"div",38),o(6),E(7,"truncate"),t(),m(8,"scan-branch",39),t()(),i(9,"td"),o(10),t(),i(11,"td",40),o(12),t(),i(13,"td"),m(14,"scan-status",34),t(),i(15,"td",41)(16,"div",42)(17,"p",43),o(18),t(),i(19,"p",44),o(20),t(),i(21,"p",45),o(22),t(),i(23,"p",46),o(24),t(),i(25,"p",47),o(26),t()()(),i(27,"td")(28,"div",48)(29,"span",49),o(30),E(31,"timeago"),t()()(),i(32,"td"),o(33),t(),i(34,"td")(35,"div",50)(36,"a",51),m(37,"ng-icon",52),t(),i(38,"a",53),m(39,"ng-icon",54),t()()()()),e&2){let n,c,a,l=p.$implicit,u=v(2);r(4),s("routerLink",_(26,L,u.store.projectId()))("queryParams",k(28,fe,l.commitId,l.scannerId)),r(),s("tooltip",l.commitTitle),r(),f(G(7,21,l.commitTitle,50)),r(2),s("action",(n=l.gitAction)!==null&&n!==void 0?n:u.GitAction.CommitBranch)("branch",(c=l.commitBranch)!==null&&c!==void 0?c:"")("targetBranch",l.targetBranch),r(2),f(l.scanner),r(2),f(l.type),r(2),s("status",(a=l.status)!==null&&a!==void 0?a:u.ScanStatus.Error),r(4),h(" ",l.severityCritical," "),r(2),h(" ",l.severityHigh," "),r(2),h(" ",l.severityMedium," "),r(2),h(" ",l.severityLow," "),r(2),h(" ",l.severityInfo," "),r(3),s("tooltip",l.startedAt),r(),f(P(31,24,l.startedAt)),r(3),f(u.duration(l.startedAt,l.completedAt)),r(3),s("routerLink",_(31,L,u.store.projectId()))("queryParams",k(33,xe,l.id,l.commitBranch)),r(2),s("href",l.jobUrl,j)}}function Ce(e,p){if(e&1&&(i(0,"tbody"),q(1,ye,40,36,"tr",null,de),t()),e&2){let n=v();r(),z(n.response.items)}}function be(e,p){e&1&&(i(0,"div",28),m(1,"div",55)(2,"div",55)(3,"div",55)(4,"div",55)(5,"div",55)(6,"div",55)(7,"div",55)(8,"div",55)(9,"div",55),t())}var We=(()=>{class e{projectService;store;route;loading=!0;statistic={severitySast:{critical:0,high:0,info:0,low:0,medium:0},severitySca:{critical:0,high:0,info:0,low:0,medium:0},statusSast:{acceptedRisk:0,confirmed:0,fixed:0,open:0},statusSca:{acceptedRisk:0,confirmed:0,fixed:0,open:0}};statisticLoading=!1;response={count:0,currentPage:1,pageCount:0,items:[],size:0};filter={size:20,page:1,desc:!0,scanner:null};constructor(n,c,a){this.projectService=n,this.store=c,this.route=a}ngOnInit(){this.statisticLoading=!0,this.projectService.getProjectStatistic({projectId:this.store.projectId()}).pipe(T(()=>{this.statisticLoading=!1})).subscribe(n=>{this.statistic=n}),this.route.queryParams.pipe(N(n=>(this.loading=!0,ne(n,this.filter),this.projectService.getProjectScans({projectId:this.store.projectId(),body:this.filter}).pipe(T(()=>{this.loading=!1})))),R(this.destroy$)).subscribe(n=>{this.response=n})}onSearchChange(){}onStatusChange(n){}duration(n,c){if(c==null||n==null)return"-";let a=new Date(n),u=new Date(c).getTime()-a.getTime(),M=Math.floor(u/1e3%60),A=Math.floor(u/(1e3*60)%60),F=Math.floor(u/(1e3*60*60)%24),D=Math.floor(u/(1e3*60*60*24)),S=[];return D>0&&S.push(`${D} days`),F>0&&S.push(`${F} hours`),A>0&&S.push(`${A} minutes`),M>0&&S.push(`${M} seconds`),S.length>0?S.join(", "):"0 seconds"}mIcon=new Map([[y.CommitTag,"gitTag"],[y.CommitBranch,"gitBranch"],[y.MergeRequest,"gitMerge"]]);ngOnDestroy(){this.destroy$.next(null),this.destroy$.complete()}destroy$=new B;GitAction=y;Date=Date;ScanStatus=d;static \u0275fac=function(c){return new(c||e)(b(oe),b(ae),b(H))};static \u0275cmp=C({type:e,selectors:[["app-scan"]],standalone:!0,features:[w],decls:64,vars:11,consts:[[1,"mx-auto","px-2","lg:px-6","flex","flex-col","pt-6","text-sm","gap-4"],[1,"flex","flex-col","overflow-hidden","w-full","border","border-white/5","gap-4","rounded-xl","bg-background","p-6"],[1,"flex","w-full","items-center"],[1,"flex","flex-col","font-normal","gap-2"],[1,"flex","flex-row","gap-2","items-center","text-base"],[3,"name"],["target","_blank",1,"hover:underline",3,"href"],[1,"flex","flex-row","items-center","gap-1"],["name","spin",1,"animate-spin"],[1,"font-bold","text-primary","hover:underline","hover:underline-offset-4",3,"routerLink","queryParams"],[1,"flex","flex-wrap"],[3,"row"],[1,"flex","min-w-full","flex-col","rounded-xl","border","border-border","bg-background"],[1,"flex","flex-col","py-3","px-5"],[1,"flex","flex-row","gap-2","w-full"],[1,"flex","w-full","md:max-w-80"],[1,"relative","text-muted-foreground","w-full"],[1,"absolute","left-2.5","top-2.5"],["name","heroMagnifyingGlass"],["placeholder","Search...","type","text",1,"py-2","pl-8","pr-2",3,"keyup.enter"],[1,"overflow-x-auto"],[1,"table","w-full","min-w-[1024px]","table-auto","border-collapse","border-0","text-left","align-middle","leading-5","text-muted-foreground"],[1,"border-t","border-border","text-xs","text-muted-foreground"],[1,"w-[50px]","min-w-[50px]","text-center"],["data-datatable-check","true","type","checkbox",1,"checkbox","checkbox-sm"],[1,"flex","flex-row","items-center","gap-1","min-w-32"],[1,"flex","flex-row","items-center","gap-1","w-10"],[1,"max-w-10"],[1,"flex","flex-col","animate-pulse","w-full","gap-0.5"],[1,"sticky","bottom-0","z-10",3,"currentPage","totalPage"],[1,"flex","flex-col","items-center","gap-4"],[1,"font-semibold"],[1,"flex","flex-wrap","gap-4"],["severity-chart","",3,"severity"],[3,"status"],[1,"text-center"],["data-datatable-row-check","true","type","checkbox","value","28",1,"checkbox","checkbox-sm"],[1,"flex","flex-col","gap-1",3,"routerLink","queryParams"],["position","dynamic",3,"tooltip"],[3,"action","branch","targetBranch"],[1,"lowercase"],[1,"min-w-32"],[1,"mr-4","font-mono","text-xs","rounded-md","overflow-hidden","gap-1","shadow-sm","shadow-black/20","h-[28px]","flex","items-stretch"],[1,"flex-1","text-center","bg-rose-600/20","text-rose-500","flex","items-center","justify-center","opacity-100"],[1,"flex-1","text-center","bg-orange-600/20","text-orange-500","flex","items-center","justify-center","opacity-100"],[1,"flex-1","text-center","bg-yellow-600/20","text-yellow-500","flex","items-center","justify-center","opacity-100"],[1,"flex-1","text-center","bg-indigo-500/20","text-indigo-400","flex","items-center","justify-center","opacity-100"],[1,"flex-1","text-center","bg-green-500/20","text-green-400","flex","items-center","justify-center","opacity-100"],[1,"text-[#00c869]"],[3,"tooltip"],[1,"flex","flex-row","items-center","gap-2"],[1,"cursor-pointer",3,"routerLink","queryParams"],["name","arrowTopRightOnSquare","size","18"],["target","_blank",1,"cursor-pointer",3,"href"],["name","commandLine","size","20"],[1,"h-12","bg-gray-200","dark:bg-gray-700/40"]],template:function(c,a){c&1&&(i(0,"div",0)(1,"div",1)(2,"div",2)(3,"div",3)(4,"div",4),m(5,"ng-icon",5),E(6,"lowercase"),i(7,"a",6),o(8),t()(),i(9,"div",7)(10,"div"),o(11,"You have"),t(),x(12,ge,1,0,"ng-icon",8)(13,ve,2,6,"a",9),i(14,"div"),o(15,"open vulnerabilities."),t()()()(),i(16,"div",10),x(17,he,1,1,"loading-table",11)(18,Se,12,4),t()(),i(19,"div",12)(20,"div",13)(21,"div",14)(22,"div",15)(23,"label",16)(24,"div",17),m(25,"ng-icon",18),t(),i(26,"input",19),U("keyup.enter",function(){return a.onSearchChange()}),t()()()()(),i(27,"div",20)(28,"table",21)(29,"thead",22)(30,"tr")(31,"th",23),m(32,"input",24),t(),i(33,"th")(34,"div",7)(35,"span"),o(36,"SCAN"),t()()(),i(37,"th")(38,"div",7)(39,"span"),o(40,"SCANNER"),t()()(),i(41,"th")(42,"div",7)(43,"span"),o(44,"TYPE"),t()()(),i(45,"th")(46,"div",7)(47,"span"),o(48,"STATUS"),t()()(),i(49,"th")(50,"div",25)(51,"span"),o(52,"FINDING"),t()()(),i(53,"th")(54,"div",26)(55,"span"),o(56,"LAST SCAN"),t()()(),i(57,"th",27),o(58," DURATION "),t(),i(59,"th",27),o(60,"ACTION"),t()()(),x(61,Ce,3,0,"tbody"),t()(),x(62,be,10,0,"div",28),m(63,"pagination",29),t()()),c&2&&(r(5),s("name",P(6,9,a.store.project().sourceType)),r(2),s("href",a.store.project().repoUrl,j),r(),f(a.store.project().name),r(4),g(a.statisticLoading?12:13),r(5),g(a.statisticLoading?17:18),r(44),g(a.loading?-1:61),r(),g(a.loading?62:-1),r(),s("currentPage",a.response.currentPage)("totalPage",a.response.pageCount))},dependencies:[I,V,X,ee,Z,W,J,le,se,re,ie,te,K,Y,me]})}return e})();export{We as ScanComponent};
