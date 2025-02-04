import{a as re}from"./chunk-3OH2LOAZ.js";import{$a as u,Pb as f,S as T,Ta as o,U as h,W as j,Xa as te,Y as v,Za as D,Zb as ve,ca as B,da as l,eb as ie,ec as ye,f as pe,gc as Ce,hb as ne,hc as A,ic as p,k as ge,ka as I,p as me,pa as V,ta as y,v as _e,wb as b,ya as ee}from"./chunk-LUMD5QXA.js";import{a as c,b as d}from"./chunk-AIZVJUQQ.js";var Ie=(()=>{class i{constructor(e,n){this._renderer=e,this._elementRef=n,this.onChange=r=>{},this.onTouched=()=>{}}setProperty(e,n){this._renderer.setProperty(this._elementRef.nativeElement,e,n)}registerOnTouched(e){this.onTouched=e}registerOnChange(e){this.onChange=e}setDisabledState(e){this.setProperty("disabled",e)}static{this.\u0275fac=function(n){return new(n||i)(o(te),o(ee))}}static{this.\u0275dir=l({type:i})}}return i})(),le=(()=>{class i extends Ie{static{this.\u0275fac=(()=>{let e;return function(r){return(e||(e=V(i)))(r||i)}})()}static{this.\u0275dir=l({type:i,features:[u]})}}return i})(),R=new v(""),tt={provide:R,useExisting:h(()=>it),multi:!0},it=(()=>{class i extends le{writeValue(e){this.setProperty("checked",e)}static{this.\u0275fac=(()=>{let e;return function(r){return(e||(e=V(i)))(r||i)}})()}static{this.\u0275dir=l({type:i,selectors:[["input","type","checkbox","formControlName",""],["input","type","checkbox","formControl",""],["input","type","checkbox","ngModel",""]],hostBindings:function(n,r){n&1&&b("change",function(a){return r.onChange(a.target.checked)})("blur",function(){return r.onTouched()})},features:[f([tt]),u]})}}return i})(),nt={provide:R,useExisting:h(()=>Se),multi:!0};function rt(){let i=re()?re().getUserAgent():"";return/android (\d+)/.test(i.toLowerCase())}var st=new v(""),Se=(()=>{class i extends Ie{constructor(e,n,r){super(e,n),this._compositionMode=r,this._composing=!1,this._compositionMode==null&&(this._compositionMode=!rt())}writeValue(e){let n=e??"";this.setProperty("value",n)}_handleInput(e){(!this._compositionMode||this._compositionMode&&!this._composing)&&this.onChange(e)}_compositionStart(){this._composing=!0}_compositionEnd(e){this._composing=!1,this._compositionMode&&this.onChange(e)}static{this.\u0275fac=function(n){return new(n||i)(o(te),o(ee),o(st,8))}}static{this.\u0275dir=l({type:i,selectors:[["input","formControlName","",3,"type","checkbox"],["textarea","formControlName",""],["input","formControl","",3,"type","checkbox"],["textarea","formControl",""],["input","ngModel","",3,"type","checkbox"],["textarea","ngModel",""],["","ngDefaultControl",""]],hostBindings:function(n,r){n&1&&b("input",function(a){return r._handleInput(a.target.value)})("blur",function(){return r.onTouched()})("compositionstart",function(){return r._compositionStart()})("compositionend",function(a){return r._compositionEnd(a.target.value)})},features:[f([nt]),u]})}}return i})();function g(i){return i==null||(typeof i=="string"||Array.isArray(i))&&i.length===0}function Ne(i){return i!=null&&typeof i.length=="number"}var F=new v(""),K=new v(""),ot=/^(?=.{1,254}$)(?=.{1,64}@)[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/,Ve=class{static min(t){return Oe(t)}static max(t){return at(t)}static required(t){return lt(t)}static requiredTrue(t){return ut(t)}static email(t){return ct(t)}static minLength(t){return dt(t)}static maxLength(t){return xe(t)}static pattern(t){return ht(t)}static nullValidator(t){return H(t)}static compose(t){return je(t)}static composeAsync(t){return Be(t)}};function Oe(i){return t=>{if(g(t.value)||g(i))return null;let e=parseFloat(t.value);return!isNaN(e)&&e<i?{min:{min:i,actual:t.value}}:null}}function at(i){return t=>{if(g(t.value)||g(i))return null;let e=parseFloat(t.value);return!isNaN(e)&&e>i?{max:{max:i,actual:t.value}}:null}}function lt(i){return g(i.value)?{required:!0}:null}function ut(i){return i.value===!0?null:{required:!0}}function ct(i){return g(i.value)||ot.test(i.value)?null:{email:!0}}function dt(i){return t=>g(t.value)||!Ne(t.value)?null:t.value.length<i?{minlength:{requiredLength:i,actualLength:t.value.length}}:null}function xe(i){return t=>Ne(t.value)&&t.value.length>i?{maxlength:{requiredLength:i,actualLength:t.value.length}}:null}function ht(i){if(!i)return H;let t,e;return typeof i=="string"?(e="",i.charAt(0)!=="^"&&(e+="^"),e+=i,i.charAt(i.length-1)!=="$"&&(e+="$"),t=new RegExp(e)):(e=i.toString(),t=i),n=>{if(g(n.value))return null;let r=n.value;return t.test(r)?null:{pattern:{requiredPattern:e,actualValue:r}}}}function H(i){return null}function Pe(i){return i!=null}function ke(i){return ve(i)?ge(i):i}function Ge(i){let t={};return i.forEach(e=>{t=e!=null?c(c({},t),e):t}),Object.keys(t).length===0?null:t}function Re(i,t){return t.map(e=>e(i))}function ft(i){return!i.validate}function Te(i){return i.map(t=>ft(t)?t:e=>t.validate(e))}function je(i){if(!i)return null;let t=i.filter(Pe);return t.length==0?null:function(e){return Ge(Re(e,t))}}function ue(i){return i!=null?je(Te(i)):null}function Be(i){if(!i)return null;let t=i.filter(Pe);return t.length==0?null:function(e){let n=Re(e,t).map(ke);return _e(n).pipe(me(Ge))}}function ce(i){return i!=null?Be(Te(i)):null}function De(i,t){return i===null?[t]:Array.isArray(i)?[...i,t]:[i,t]}function Ue(i){return i._rawValidators}function He(i){return i._rawAsyncValidators}function se(i){return i?Array.isArray(i)?i:[i]:[]}function L(i,t){return Array.isArray(i)?i.includes(t):i===t}function be(i,t){let e=se(t);return se(i).forEach(r=>{L(e,r)||e.push(r)}),e}function Ae(i,t){return se(t).filter(e=>!L(i,e))}var $=class{constructor(){this._rawValidators=[],this._rawAsyncValidators=[],this._onDestroyCallbacks=[]}get value(){return this.control?this.control.value:null}get valid(){return this.control?this.control.valid:null}get invalid(){return this.control?this.control.invalid:null}get pending(){return this.control?this.control.pending:null}get disabled(){return this.control?this.control.disabled:null}get enabled(){return this.control?this.control.enabled:null}get errors(){return this.control?this.control.errors:null}get pristine(){return this.control?this.control.pristine:null}get dirty(){return this.control?this.control.dirty:null}get touched(){return this.control?this.control.touched:null}get status(){return this.control?this.control.status:null}get untouched(){return this.control?this.control.untouched:null}get statusChanges(){return this.control?this.control.statusChanges:null}get valueChanges(){return this.control?this.control.valueChanges:null}get path(){return null}_setValidators(t){this._rawValidators=t||[],this._composedValidatorFn=ue(this._rawValidators)}_setAsyncValidators(t){this._rawAsyncValidators=t||[],this._composedAsyncValidatorFn=ce(this._rawAsyncValidators)}get validator(){return this._composedValidatorFn||null}get asyncValidator(){return this._composedAsyncValidatorFn||null}_registerOnDestroy(t){this._onDestroyCallbacks.push(t)}_invokeOnDestroyCallbacks(){this._onDestroyCallbacks.forEach(t=>t()),this._onDestroyCallbacks=[]}reset(t=void 0){this.control&&this.control.reset(t)}hasError(t,e){return this.control?this.control.hasError(t,e):!1}getError(t,e){return this.control?this.control.getError(t,e):null}},m=class extends ${get formDirective(){return null}get path(){return null}},C=class extends ${constructor(){super(...arguments),this._parent=null,this.name=null,this.valueAccessor=null}},W=class{constructor(t){this._cd=t}get isTouched(){return this._cd?.control?._touched?.(),!!this._cd?.control?.touched}get isUntouched(){return!!this._cd?.control?.untouched}get isPristine(){return this._cd?.control?._pristine?.(),!!this._cd?.control?.pristine}get isDirty(){return!!this._cd?.control?.dirty}get isValid(){return this._cd?.control?._status?.(),!!this._cd?.control?.valid}get isInvalid(){return!!this._cd?.control?.invalid}get isPending(){return!!this._cd?.control?.pending}get isSubmitted(){return this._cd?._submitted?.(),!!this._cd?.submitted}},pt={"[class.ng-untouched]":"isUntouched","[class.ng-touched]":"isTouched","[class.ng-pristine]":"isPristine","[class.ng-dirty]":"isDirty","[class.ng-valid]":"isValid","[class.ng-invalid]":"isInvalid","[class.ng-pending]":"isPending"},li=d(c({},pt),{"[class.ng-submitted]":"isSubmitted"}),ui=(()=>{class i extends W{constructor(e){super(e)}static{this.\u0275fac=function(n){return new(n||i)(o(C,2))}}static{this.\u0275dir=l({type:i,selectors:[["","formControlName",""],["","ngModel",""],["","formControl",""]],hostVars:14,hostBindings:function(n,r){n&2&&ne("ng-untouched",r.isUntouched)("ng-touched",r.isTouched)("ng-pristine",r.isPristine)("ng-dirty",r.isDirty)("ng-valid",r.isValid)("ng-invalid",r.isInvalid)("ng-pending",r.isPending)},features:[u]})}}return i})(),ci=(()=>{class i extends W{constructor(e){super(e)}static{this.\u0275fac=function(n){return new(n||i)(o(m,10))}}static{this.\u0275dir=l({type:i,selectors:[["","formGroupName",""],["","formArrayName",""],["","ngModelGroup",""],["","formGroup",""],["form",3,"ngNoForm",""],["","ngForm",""]],hostVars:16,hostBindings:function(n,r){n&2&&ne("ng-untouched",r.isUntouched)("ng-touched",r.isTouched)("ng-pristine",r.isPristine)("ng-dirty",r.isDirty)("ng-valid",r.isValid)("ng-invalid",r.isInvalid)("ng-pending",r.isPending)("ng-submitted",r.isSubmitted)},features:[u]})}}return i})();var S="VALID",U="INVALID",M="PENDING",N="DISABLED",_=class{},q=class extends _{constructor(t,e){super(),this.value=t,this.source=e}},x=class extends _{constructor(t,e){super(),this.pristine=t,this.source=e}},P=class extends _{constructor(t,e){super(),this.touched=t,this.source=e}},E=class extends _{constructor(t,e){super(),this.status=t,this.source=e}},oe=class extends _{constructor(t){super(),this.source=t}},ae=class extends _{constructor(t){super(),this.source=t}};function de(i){return(J(i)?i.validators:i)||null}function gt(i){return Array.isArray(i)?ue(i):i||null}function he(i,t){return(J(t)?t.asyncValidators:i)||null}function mt(i){return Array.isArray(i)?ce(i):i||null}function J(i){return i!=null&&!Array.isArray(i)&&typeof i=="object"}function Le(i,t,e){let n=i.controls;if(!(t?Object.keys(n):n).length)throw new T(1e3,"");if(!n[e])throw new T(1001,"")}function $e(i,t,e){i._forEachChild((n,r)=>{if(e[r]===void 0)throw new T(1002,"")})}var k=class{constructor(t,e){this._pendingDirty=!1,this._hasOwnPendingAsyncValidator=null,this._pendingTouched=!1,this._onCollectionChange=()=>{},this._parent=null,this._status=A(()=>this.statusReactive()),this.statusReactive=D(void 0),this._pristine=A(()=>this.pristineReactive()),this.pristineReactive=D(!0),this._touched=A(()=>this.touchedReactive()),this.touchedReactive=D(!1),this._events=new pe,this.events=this._events.asObservable(),this._onDisabledChange=[],this._assignValidators(t),this._assignAsyncValidators(e)}get validator(){return this._composedValidatorFn}set validator(t){this._rawValidators=this._composedValidatorFn=t}get asyncValidator(){return this._composedAsyncValidatorFn}set asyncValidator(t){this._rawAsyncValidators=this._composedAsyncValidatorFn=t}get parent(){return this._parent}get status(){return p(this.statusReactive)}set status(t){p(()=>this.statusReactive.set(t))}get valid(){return this.status===S}get invalid(){return this.status===U}get pending(){return this.status==M}get disabled(){return this.status===N}get enabled(){return this.status!==N}get pristine(){return p(this.pristineReactive)}set pristine(t){p(()=>this.pristineReactive.set(t))}get dirty(){return!this.pristine}get touched(){return p(this.touchedReactive)}set touched(t){p(()=>this.touchedReactive.set(t))}get untouched(){return!this.touched}get updateOn(){return this._updateOn?this._updateOn:this.parent?this.parent.updateOn:"change"}setValidators(t){this._assignValidators(t)}setAsyncValidators(t){this._assignAsyncValidators(t)}addValidators(t){this.setValidators(be(t,this._rawValidators))}addAsyncValidators(t){this.setAsyncValidators(be(t,this._rawAsyncValidators))}removeValidators(t){this.setValidators(Ae(t,this._rawValidators))}removeAsyncValidators(t){this.setAsyncValidators(Ae(t,this._rawAsyncValidators))}hasValidator(t){return L(this._rawValidators,t)}hasAsyncValidator(t){return L(this._rawAsyncValidators,t)}clearValidators(){this.validator=null}clearAsyncValidators(){this.asyncValidator=null}markAsTouched(t={}){let e=this.touched===!1;this.touched=!0;let n=t.sourceControl??this;this._parent&&!t.onlySelf&&this._parent.markAsTouched(d(c({},t),{sourceControl:n})),e&&t.emitEvent!==!1&&this._events.next(new P(!0,n))}markAllAsTouched(t={}){this.markAsTouched({onlySelf:!0,emitEvent:t.emitEvent,sourceControl:this}),this._forEachChild(e=>e.markAllAsTouched(t))}markAsUntouched(t={}){let e=this.touched===!0;this.touched=!1,this._pendingTouched=!1;let n=t.sourceControl??this;this._forEachChild(r=>{r.markAsUntouched({onlySelf:!0,emitEvent:t.emitEvent,sourceControl:n})}),this._parent&&!t.onlySelf&&this._parent._updateTouched(t,n),e&&t.emitEvent!==!1&&this._events.next(new P(!1,n))}markAsDirty(t={}){let e=this.pristine===!0;this.pristine=!1;let n=t.sourceControl??this;this._parent&&!t.onlySelf&&this._parent.markAsDirty(d(c({},t),{sourceControl:n})),e&&t.emitEvent!==!1&&this._events.next(new x(!1,n))}markAsPristine(t={}){let e=this.pristine===!1;this.pristine=!0,this._pendingDirty=!1;let n=t.sourceControl??this;this._forEachChild(r=>{r.markAsPristine({onlySelf:!0,emitEvent:t.emitEvent})}),this._parent&&!t.onlySelf&&this._parent._updatePristine(t,n),e&&t.emitEvent!==!1&&this._events.next(new x(!0,n))}markAsPending(t={}){this.status=M;let e=t.sourceControl??this;t.emitEvent!==!1&&(this._events.next(new E(this.status,e)),this.statusChanges.emit(this.status)),this._parent&&!t.onlySelf&&this._parent.markAsPending(d(c({},t),{sourceControl:e}))}disable(t={}){let e=this._parentMarkedDirty(t.onlySelf);this.status=N,this.errors=null,this._forEachChild(r=>{r.disable(d(c({},t),{onlySelf:!0}))}),this._updateValue();let n=t.sourceControl??this;t.emitEvent!==!1&&(this._events.next(new q(this.value,n)),this._events.next(new E(this.status,n)),this.valueChanges.emit(this.value),this.statusChanges.emit(this.status)),this._updateAncestors(d(c({},t),{skipPristineCheck:e}),this),this._onDisabledChange.forEach(r=>r(!0))}enable(t={}){let e=this._parentMarkedDirty(t.onlySelf);this.status=S,this._forEachChild(n=>{n.enable(d(c({},t),{onlySelf:!0}))}),this.updateValueAndValidity({onlySelf:!0,emitEvent:t.emitEvent}),this._updateAncestors(d(c({},t),{skipPristineCheck:e}),this),this._onDisabledChange.forEach(n=>n(!1))}_updateAncestors(t,e){this._parent&&!t.onlySelf&&(this._parent.updateValueAndValidity(t),t.skipPristineCheck||this._parent._updatePristine({},e),this._parent._updateTouched({},e))}setParent(t){this._parent=t}getRawValue(){return this.value}updateValueAndValidity(t={}){if(this._setInitialStatus(),this._updateValue(),this.enabled){let n=this._cancelExistingSubscription();this.errors=this._runValidator(),this.status=this._calculateStatus(),(this.status===S||this.status===M)&&this._runAsyncValidator(n,t.emitEvent)}let e=t.sourceControl??this;t.emitEvent!==!1&&(this._events.next(new q(this.value,e)),this._events.next(new E(this.status,e)),this.valueChanges.emit(this.value),this.statusChanges.emit(this.status)),this._parent&&!t.onlySelf&&this._parent.updateValueAndValidity(d(c({},t),{sourceControl:e}))}_updateTreeValidity(t={emitEvent:!0}){this._forEachChild(e=>e._updateTreeValidity(t)),this.updateValueAndValidity({onlySelf:!0,emitEvent:t.emitEvent})}_setInitialStatus(){this.status=this._allControlsDisabled()?N:S}_runValidator(){return this.validator?this.validator(this):null}_runAsyncValidator(t,e){if(this.asyncValidator){this.status=M,this._hasOwnPendingAsyncValidator={emitEvent:e!==!1};let n=ke(this.asyncValidator(this));this._asyncValidationSubscription=n.subscribe(r=>{this._hasOwnPendingAsyncValidator=null,this.setErrors(r,{emitEvent:e,shouldHaveEmitted:t})})}}_cancelExistingSubscription(){if(this._asyncValidationSubscription){this._asyncValidationSubscription.unsubscribe();let t=this._hasOwnPendingAsyncValidator?.emitEvent??!1;return this._hasOwnPendingAsyncValidator=null,t}return!1}setErrors(t,e={}){this.errors=t,this._updateControlsErrors(e.emitEvent!==!1,this,e.shouldHaveEmitted)}get(t){let e=t;return e==null||(Array.isArray(e)||(e=e.split(".")),e.length===0)?null:e.reduce((n,r)=>n&&n._find(r),this)}getError(t,e){let n=e?this.get(e):this;return n&&n.errors?n.errors[t]:null}hasError(t,e){return!!this.getError(t,e)}get root(){let t=this;for(;t._parent;)t=t._parent;return t}_updateControlsErrors(t,e,n){this.status=this._calculateStatus(),t&&this.statusChanges.emit(this.status),(t||n)&&this._events.next(new E(this.status,e)),this._parent&&this._parent._updateControlsErrors(t,e,n)}_initObservables(){this.valueChanges=new y,this.statusChanges=new y}_calculateStatus(){return this._allControlsDisabled()?N:this.errors?U:this._hasOwnPendingAsyncValidator||this._anyControlsHaveStatus(M)?M:this._anyControlsHaveStatus(U)?U:S}_anyControlsHaveStatus(t){return this._anyControls(e=>e.status===t)}_anyControlsDirty(){return this._anyControls(t=>t.dirty)}_anyControlsTouched(){return this._anyControls(t=>t.touched)}_updatePristine(t,e){let n=!this._anyControlsDirty(),r=this.pristine!==n;this.pristine=n,this._parent&&!t.onlySelf&&this._parent._updatePristine(t,e),r&&this._events.next(new x(this.pristine,e))}_updateTouched(t={},e){this.touched=this._anyControlsTouched(),this._events.next(new P(this.touched,e)),this._parent&&!t.onlySelf&&this._parent._updateTouched(t,e)}_registerOnCollectionChange(t){this._onCollectionChange=t}_setUpdateStrategy(t){J(t)&&t.updateOn!=null&&(this._updateOn=t.updateOn)}_parentMarkedDirty(t){let e=this._parent&&this._parent.dirty;return!t&&!!e&&!this._parent._anyControlsDirty()}_find(t){return null}_assignValidators(t){this._rawValidators=Array.isArray(t)?t.slice():t,this._composedValidatorFn=gt(this._rawValidators)}_assignAsyncValidators(t){this._rawAsyncValidators=Array.isArray(t)?t.slice():t,this._composedAsyncValidatorFn=mt(this._rawAsyncValidators)}},z=class extends k{constructor(t,e,n){super(de(e),he(n,e)),this.controls=t,this._initObservables(),this._setUpdateStrategy(e),this._setUpControls(),this.updateValueAndValidity({onlySelf:!0,emitEvent:!!this.asyncValidator})}registerControl(t,e){return this.controls[t]?this.controls[t]:(this.controls[t]=e,e.setParent(this),e._registerOnCollectionChange(this._onCollectionChange),e)}addControl(t,e,n={}){this.registerControl(t,e),this.updateValueAndValidity({emitEvent:n.emitEvent}),this._onCollectionChange()}removeControl(t,e={}){this.controls[t]&&this.controls[t]._registerOnCollectionChange(()=>{}),delete this.controls[t],this.updateValueAndValidity({emitEvent:e.emitEvent}),this._onCollectionChange()}setControl(t,e,n={}){this.controls[t]&&this.controls[t]._registerOnCollectionChange(()=>{}),delete this.controls[t],e&&this.registerControl(t,e),this.updateValueAndValidity({emitEvent:n.emitEvent}),this._onCollectionChange()}contains(t){return this.controls.hasOwnProperty(t)&&this.controls[t].enabled}setValue(t,e={}){$e(this,!0,t),Object.keys(t).forEach(n=>{Le(this,!0,n),this.controls[n].setValue(t[n],{onlySelf:!0,emitEvent:e.emitEvent})}),this.updateValueAndValidity(e)}patchValue(t,e={}){t!=null&&(Object.keys(t).forEach(n=>{let r=this.controls[n];r&&r.patchValue(t[n],{onlySelf:!0,emitEvent:e.emitEvent})}),this.updateValueAndValidity(e))}reset(t={},e={}){this._forEachChild((n,r)=>{n.reset(t?t[r]:null,{onlySelf:!0,emitEvent:e.emitEvent})}),this._updatePristine(e,this),this._updateTouched(e,this),this.updateValueAndValidity(e)}getRawValue(){return this._reduceChildren({},(t,e,n)=>(t[n]=e.getRawValue(),t))}_syncPendingControls(){let t=this._reduceChildren(!1,(e,n)=>n._syncPendingControls()?!0:e);return t&&this.updateValueAndValidity({onlySelf:!0}),t}_forEachChild(t){Object.keys(this.controls).forEach(e=>{let n=this.controls[e];n&&t(n,e)})}_setUpControls(){this._forEachChild(t=>{t.setParent(this),t._registerOnCollectionChange(this._onCollectionChange)})}_updateValue(){this.value=this._reduceValue()}_anyControls(t){for(let[e,n]of Object.entries(this.controls))if(this.contains(e)&&t(n))return!0;return!1}_reduceValue(){let t={};return this._reduceChildren(t,(e,n,r)=>((n.enabled||this.disabled)&&(e[r]=n.value),e))}_reduceChildren(t,e){let n=t;return this._forEachChild((r,s)=>{n=e(n,r,s)}),n}_allControlsDisabled(){for(let t of Object.keys(this.controls))if(this.controls[t].enabled)return!1;return Object.keys(this.controls).length>0||this.disabled}_find(t){return this.controls.hasOwnProperty(t)?this.controls[t]:null}};var w=new v("CallSetDisabledState",{providedIn:"root",factory:()=>Q}),Q="always";function _t(i,t){return[...t.path,i]}function G(i,t,e=Q){fe(i,t),t.valueAccessor.writeValue(i.value),(i.disabled||e==="always")&&t.valueAccessor.setDisabledState?.(i.disabled),yt(i,t),Vt(i,t),Ct(i,t),vt(i,t)}function Z(i,t,e=!0){let n=()=>{};t.valueAccessor&&(t.valueAccessor.registerOnChange(n),t.valueAccessor.registerOnTouched(n)),Y(i,t),i&&(t._invokeOnDestroyCallbacks(),i._registerOnCollectionChange(()=>{}))}function X(i,t){i.forEach(e=>{e.registerOnValidatorChange&&e.registerOnValidatorChange(t)})}function vt(i,t){if(t.valueAccessor.setDisabledState){let e=n=>{t.valueAccessor.setDisabledState(n)};i.registerOnDisabledChange(e),t._registerOnDestroy(()=>{i._unregisterOnDisabledChange(e)})}}function fe(i,t){let e=Ue(i);t.validator!==null?i.setValidators(De(e,t.validator)):typeof e=="function"&&i.setValidators([e]);let n=He(i);t.asyncValidator!==null?i.setAsyncValidators(De(n,t.asyncValidator)):typeof n=="function"&&i.setAsyncValidators([n]);let r=()=>i.updateValueAndValidity();X(t._rawValidators,r),X(t._rawAsyncValidators,r)}function Y(i,t){let e=!1;if(i!==null){if(t.validator!==null){let r=Ue(i);if(Array.isArray(r)&&r.length>0){let s=r.filter(a=>a!==t.validator);s.length!==r.length&&(e=!0,i.setValidators(s))}}if(t.asyncValidator!==null){let r=He(i);if(Array.isArray(r)&&r.length>0){let s=r.filter(a=>a!==t.asyncValidator);s.length!==r.length&&(e=!0,i.setAsyncValidators(s))}}}let n=()=>{};return X(t._rawValidators,n),X(t._rawAsyncValidators,n),e}function yt(i,t){t.valueAccessor.registerOnChange(e=>{i._pendingValue=e,i._pendingChange=!0,i._pendingDirty=!0,i.updateOn==="change"&&We(i,t)})}function Ct(i,t){t.valueAccessor.registerOnTouched(()=>{i._pendingTouched=!0,i.updateOn==="blur"&&i._pendingChange&&We(i,t),i.updateOn!=="submit"&&i.markAsTouched()})}function We(i,t){i._pendingDirty&&i.markAsDirty(),i.setValue(i._pendingValue,{emitModelToViewChange:!1}),t.viewToModelUpdate(i._pendingValue),i._pendingChange=!1}function Vt(i,t){let e=(n,r)=>{t.valueAccessor.writeValue(n),r&&t.viewToModelUpdate(n)};i.registerOnChange(e),t._registerOnDestroy(()=>{i._unregisterOnChange(e)})}function qe(i,t){i==null,fe(i,t)}function Dt(i,t){return Y(i,t)}function ze(i,t){if(!i.hasOwnProperty("model"))return!1;let e=i.model;return e.isFirstChange()?!0:!Object.is(t,e.currentValue)}function bt(i){return Object.getPrototypeOf(i.constructor)===le}function Ze(i,t){i._syncPendingControls(),t.forEach(e=>{let n=e.control;n.updateOn==="submit"&&n._pendingChange&&(e.viewToModelUpdate(n._pendingValue),n._pendingChange=!1)})}function Xe(i,t){if(!t)return null;Array.isArray(t);let e,n,r;return t.forEach(s=>{s.constructor===Se?e=s:bt(s)?n=s:r=s}),r||n||e||null}function At(i,t){let e=i.indexOf(t);e>-1&&i.splice(e,1)}var Mt={provide:m,useExisting:h(()=>Et)},O=Promise.resolve(),Et=(()=>{class i extends m{get submitted(){return p(this.submittedReactive)}constructor(e,n,r){super(),this.callSetDisabledState=r,this._submitted=A(()=>this.submittedReactive()),this.submittedReactive=D(!1),this._directives=new Set,this.ngSubmit=new y,this.form=new z({},ue(e),ce(n))}ngAfterViewInit(){this._setUpdateStrategy()}get formDirective(){return this}get control(){return this.form}get path(){return[]}get controls(){return this.form.controls}addControl(e){O.then(()=>{let n=this._findContainer(e.path);e.control=n.registerControl(e.name,e.control),G(e.control,e,this.callSetDisabledState),e.control.updateValueAndValidity({emitEvent:!1}),this._directives.add(e)})}getControl(e){return this.form.get(e.path)}removeControl(e){O.then(()=>{let n=this._findContainer(e.path);n&&n.removeControl(e.name),this._directives.delete(e)})}addFormGroup(e){O.then(()=>{let n=this._findContainer(e.path),r=new z({});qe(r,e),n.registerControl(e.name,r),r.updateValueAndValidity({emitEvent:!1})})}removeFormGroup(e){O.then(()=>{let n=this._findContainer(e.path);n&&n.removeControl(e.name)})}getFormGroup(e){return this.form.get(e.path)}updateModel(e,n){O.then(()=>{this.form.get(e.path).setValue(n)})}setValue(e){this.control.setValue(e)}onSubmit(e){return this.submittedReactive.set(!0),Ze(this.form,this._directives),this.ngSubmit.emit(e),e?.target?.method==="dialog"}onReset(){this.resetForm()}resetForm(e=void 0){this.form.reset(e),this.submittedReactive.set(!1)}_setUpdateStrategy(){this.options&&this.options.updateOn!=null&&(this.form._updateOn=this.options.updateOn)}_findContainer(e){return e.pop(),e.length?this.form.get(e):this.form}static{this.\u0275fac=function(n){return new(n||i)(o(F,10),o(K,10),o(w,8))}}static{this.\u0275dir=l({type:i,selectors:[["form",3,"ngNoForm","",3,"formGroup",""],["ng-form"],["","ngForm",""]],hostBindings:function(n,r){n&1&&b("submit",function(a){return r.onSubmit(a)})("reset",function(){return r.onReset()})},inputs:{options:[0,"ngFormOptions","options"]},outputs:{ngSubmit:"ngSubmit"},exportAs:["ngForm"],features:[f([Mt]),u]})}}return i})();function Me(i,t){let e=i.indexOf(t);e>-1&&i.splice(e,1)}function Ee(i){return typeof i=="object"&&i!==null&&Object.keys(i).length===2&&"value"in i&&"disabled"in i}var Ye=class extends k{constructor(t=null,e,n){super(de(e),he(n,e)),this.defaultValue=null,this._onChange=[],this._pendingChange=!1,this._applyFormState(t),this._setUpdateStrategy(e),this._initObservables(),this.updateValueAndValidity({onlySelf:!0,emitEvent:!!this.asyncValidator}),J(e)&&(e.nonNullable||e.initialValueIsDefault)&&(Ee(t)?this.defaultValue=t.value:this.defaultValue=t)}setValue(t,e={}){this.value=this._pendingValue=t,this._onChange.length&&e.emitModelToViewChange!==!1&&this._onChange.forEach(n=>n(this.value,e.emitViewToModelChange!==!1)),this.updateValueAndValidity(e)}patchValue(t,e={}){this.setValue(t,e)}reset(t=this.defaultValue,e={}){this._applyFormState(t),this.markAsPristine(e),this.markAsUntouched(e),this.setValue(this.value,e),this._pendingChange=!1}_updateValue(){}_anyControls(t){return!1}_allControlsDisabled(){return this.disabled}registerOnChange(t){this._onChange.push(t)}_unregisterOnChange(t){Me(this._onChange,t)}registerOnDisabledChange(t){this._onDisabledChange.push(t)}_unregisterOnDisabledChange(t){Me(this._onDisabledChange,t)}_forEachChild(t){}_syncPendingControls(){return this.updateOn==="submit"&&(this._pendingDirty&&this.markAsDirty(),this._pendingTouched&&this.markAsTouched(),this._pendingChange)?(this.setValue(this._pendingValue,{onlySelf:!0,emitModelToViewChange:!1}),!0):!1}_applyFormState(t){Ee(t)?(this.value=this._pendingValue=t.value,t.disabled?this.disable({onlySelf:!0,emitEvent:!1}):this.enable({onlySelf:!0,emitEvent:!1})):this.value=this._pendingValue=t}};var Ft=i=>i instanceof Ye;var wt={provide:C,useExisting:h(()=>It)},Fe=Promise.resolve(),It=(()=>{class i extends C{constructor(e,n,r,s,a,et){super(),this._changeDetectorRef=a,this.callSetDisabledState=et,this.control=new Ye,this._registered=!1,this.name="",this.update=new y,this._parent=e,this._setValidators(n),this._setAsyncValidators(r),this.valueAccessor=Xe(this,s)}ngOnChanges(e){if(this._checkForErrors(),!this._registered||"name"in e){if(this._registered&&(this._checkName(),this.formDirective)){let n=e.name.previousValue;this.formDirective.removeControl({name:n,path:this._getPath(n)})}this._setUpControl()}"isDisabled"in e&&this._updateDisabled(e),ze(e,this.viewModel)&&(this._updateValue(this.model),this.viewModel=this.model)}ngOnDestroy(){this.formDirective&&this.formDirective.removeControl(this)}get path(){return this._getPath(this.name)}get formDirective(){return this._parent?this._parent.formDirective:null}viewToModelUpdate(e){this.viewModel=e,this.update.emit(e)}_setUpControl(){this._setUpdateStrategy(),this._isStandalone()?this._setUpStandalone():this.formDirective.addControl(this),this._registered=!0}_setUpdateStrategy(){this.options&&this.options.updateOn!=null&&(this.control._updateOn=this.options.updateOn)}_isStandalone(){return!this._parent||!!(this.options&&this.options.standalone)}_setUpStandalone(){G(this.control,this,this.callSetDisabledState),this.control.updateValueAndValidity({emitEvent:!1})}_checkForErrors(){this._isStandalone()||this._checkParentType(),this._checkName()}_checkParentType(){}_checkName(){this.options&&this.options.name&&(this.name=this.options.name),!this._isStandalone()&&this.name}_updateValue(e){Fe.then(()=>{this.control.setValue(e,{emitViewToModelChange:!1}),this._changeDetectorRef?.markForCheck()})}_updateDisabled(e){let n=e.isDisabled.currentValue,r=n!==0&&Ce(n);Fe.then(()=>{r&&!this.control.disabled?this.control.disable():!r&&this.control.disabled&&this.control.enable(),this._changeDetectorRef?.markForCheck()})}_getPath(e){return this._parent?_t(e,this._parent):[e]}static{this.\u0275fac=function(n){return new(n||i)(o(m,9),o(F,10),o(K,10),o(R,10),o(ye,8),o(w,8))}}static{this.\u0275dir=l({type:i,selectors:[["","ngModel","",3,"formControlName","",3,"formControl",""]],inputs:{name:"name",isDisabled:[0,"disabled","isDisabled"],model:[0,"ngModel","model"],options:[0,"ngModelOptions","options"]},outputs:{update:"ngModelChange"},exportAs:["ngModel"],features:[f([wt]),u,I]})}}return i})(),hi=(()=>{class i{static{this.\u0275fac=function(n){return new(n||i)}}static{this.\u0275dir=l({type:i,selectors:[["form",3,"ngNoForm","",3,"ngNativeValidate",""]],hostAttrs:["novalidate",""]})}}return i})(),St={provide:R,useExisting:h(()=>Nt),multi:!0},Nt=(()=>{class i extends le{writeValue(e){let n=e??"";this.setProperty("value",n)}registerOnChange(e){this.onChange=n=>{e(n==""?null:parseFloat(n))}}static{this.\u0275fac=(()=>{let e;return function(r){return(e||(e=V(i)))(r||i)}})()}static{this.\u0275dir=l({type:i,selectors:[["input","type","number","formControlName",""],["input","type","number","formControl",""],["input","type","number","ngModel",""]],hostBindings:function(n,r){n&1&&b("input",function(a){return r.onChange(a.target.value)})("blur",function(){return r.onTouched()})},features:[f([St]),u]})}}return i})();var Ke=new v(""),Ot={provide:C,useExisting:h(()=>xt)},xt=(()=>{class i extends C{set isDisabled(e){}static{this._ngModelWarningSentOnce=!1}constructor(e,n,r,s,a){super(),this._ngModelWarningConfig=s,this.callSetDisabledState=a,this.update=new y,this._ngModelWarningSent=!1,this._setValidators(e),this._setAsyncValidators(n),this.valueAccessor=Xe(this,r)}ngOnChanges(e){if(this._isControlChanged(e)){let n=e.form.previousValue;n&&Z(n,this,!1),G(this.form,this,this.callSetDisabledState),this.form.updateValueAndValidity({emitEvent:!1})}ze(e,this.viewModel)&&(this.form.setValue(this.model),this.viewModel=this.model)}ngOnDestroy(){this.form&&Z(this.form,this,!1)}get path(){return[]}get control(){return this.form}viewToModelUpdate(e){this.viewModel=e,this.update.emit(e)}_isControlChanged(e){return e.hasOwnProperty("form")}static{this.\u0275fac=function(n){return new(n||i)(o(F,10),o(K,10),o(R,10),o(Ke,8),o(w,8))}}static{this.\u0275dir=l({type:i,selectors:[["","formControl",""]],inputs:{form:[0,"formControl","form"],isDisabled:[0,"disabled","isDisabled"],model:[0,"ngModel","model"]},outputs:{update:"ngModelChange"},exportAs:["ngForm"],features:[f([Ot]),u,I]})}}return i})(),Pt={provide:m,useExisting:h(()=>kt)},kt=(()=>{class i extends m{get submitted(){return p(this._submittedReactive)}set submitted(e){this._submittedReactive.set(e)}constructor(e,n,r){super(),this.callSetDisabledState=r,this._submitted=A(()=>this._submittedReactive()),this._submittedReactive=D(!1),this._onCollectionChange=()=>this._updateDomValue(),this.directives=[],this.form=null,this.ngSubmit=new y,this._setValidators(e),this._setAsyncValidators(n)}ngOnChanges(e){this._checkFormPresent(),e.hasOwnProperty("form")&&(this._updateValidators(),this._updateDomValue(),this._updateRegistrations(),this._oldForm=this.form)}ngOnDestroy(){this.form&&(Y(this.form,this),this.form._onCollectionChange===this._onCollectionChange&&this.form._registerOnCollectionChange(()=>{}))}get formDirective(){return this}get control(){return this.form}get path(){return[]}addControl(e){let n=this.form.get(e.path);return G(n,e,this.callSetDisabledState),n.updateValueAndValidity({emitEvent:!1}),this.directives.push(e),n}getControl(e){return this.form.get(e.path)}removeControl(e){Z(e.control||null,e,!1),At(this.directives,e)}addFormGroup(e){this._setUpFormContainer(e)}removeFormGroup(e){this._cleanUpFormContainer(e)}getFormGroup(e){return this.form.get(e.path)}addFormArray(e){this._setUpFormContainer(e)}removeFormArray(e){this._cleanUpFormContainer(e)}getFormArray(e){return this.form.get(e.path)}updateModel(e,n){this.form.get(e.path).setValue(n)}onSubmit(e){return this._submittedReactive.set(!0),Ze(this.form,this.directives),this.ngSubmit.emit(e),this.form._events.next(new oe(this.control)),e?.target?.method==="dialog"}onReset(){this.resetForm()}resetForm(e=void 0){this.form.reset(e),this._submittedReactive.set(!1),this.form._events.next(new ae(this.form))}_updateDomValue(){this.directives.forEach(e=>{let n=e.control,r=this.form.get(e.path);n!==r&&(Z(n||null,e),Ft(r)&&(G(r,e,this.callSetDisabledState),e.control=r))}),this.form._updateTreeValidity({emitEvent:!1})}_setUpFormContainer(e){let n=this.form.get(e.path);qe(n,e),n.updateValueAndValidity({emitEvent:!1})}_cleanUpFormContainer(e){if(this.form){let n=this.form.get(e.path);n&&Dt(n,e)&&n.updateValueAndValidity({emitEvent:!1})}}_updateRegistrations(){this.form._registerOnCollectionChange(this._onCollectionChange),this._oldForm&&this._oldForm._registerOnCollectionChange(()=>{})}_updateValidators(){fe(this.form,this),this._oldForm&&Y(this._oldForm,this)}_checkFormPresent(){this.form}static{this.\u0275fac=function(n){return new(n||i)(o(F,10),o(K,10),o(w,8))}}static{this.\u0275dir=l({type:i,selectors:[["","formGroup",""]],hostBindings:function(n,r){n&1&&b("submit",function(a){return r.onSubmit(a)})("reset",function(){return r.onReset()})},inputs:{form:[0,"formGroup","form"]},outputs:{ngSubmit:"ngSubmit"},exportAs:["ngForm"],features:[f([Pt]),u,I]})}}return i})();function Gt(i){return typeof i=="number"?i:parseInt(i,10)}function Rt(i){return typeof i=="number"?i:parseFloat(i)}var Je=(()=>{class i{constructor(){this._validator=H}ngOnChanges(e){if(this.inputName in e){let n=this.normalizeInput(e[this.inputName].currentValue);this._enabled=this.enabled(n),this._validator=this._enabled?this.createValidator(n):H,this._onChange&&this._onChange()}}validate(e){return this._validator(e)}registerOnValidatorChange(e){this._onChange=e}enabled(e){return e!=null}static{this.\u0275fac=function(n){return new(n||i)}}static{this.\u0275dir=l({type:i,features:[I]})}}return i})();var Tt={provide:F,useExisting:h(()=>jt),multi:!0},jt=(()=>{class i extends Je{constructor(){super(...arguments),this.inputName="min",this.normalizeInput=e=>Rt(e),this.createValidator=e=>Oe(e)}static{this.\u0275fac=(()=>{let e;return function(r){return(e||(e=V(i)))(r||i)}})()}static{this.\u0275dir=l({type:i,selectors:[["input","type","number","min","","formControlName",""],["input","type","number","min","","formControl",""],["input","type","number","min","","ngModel",""]],hostVars:1,hostBindings:function(n,r){n&2&&ie("min",r._enabled?r.min:null)},inputs:{min:"min"},features:[f([Tt]),u]})}}return i})();var Bt={provide:F,useExisting:h(()=>Ut),multi:!0},Ut=(()=>{class i extends Je{constructor(){super(...arguments),this.inputName="maxlength",this.normalizeInput=e=>Gt(e),this.createValidator=e=>xe(e)}static{this.\u0275fac=(()=>{let e;return function(r){return(e||(e=V(i)))(r||i)}})()}static{this.\u0275dir=l({type:i,selectors:[["","maxlength","","formControlName",""],["","maxlength","","formControl",""],["","maxlength","","ngModel",""]],hostVars:1,hostBindings:function(n,r){n&2&&ie("maxlength",r._enabled?r.maxlength:null)},inputs:{maxlength:"maxlength"},features:[f([Bt]),u]})}}return i})();var Qe=(()=>{class i{static{this.\u0275fac=function(n){return new(n||i)}}static{this.\u0275mod=B({type:i})}static{this.\u0275inj=j({})}}return i})(),we=class extends k{constructor(t,e,n){super(de(e),he(n,e)),this.controls=t,this._initObservables(),this._setUpdateStrategy(e),this._setUpControls(),this.updateValueAndValidity({onlySelf:!0,emitEvent:!!this.asyncValidator})}at(t){return this.controls[this._adjustIndex(t)]}push(t,e={}){this.controls.push(t),this._registerControl(t),this.updateValueAndValidity({emitEvent:e.emitEvent}),this._onCollectionChange()}insert(t,e,n={}){this.controls.splice(t,0,e),this._registerControl(e),this.updateValueAndValidity({emitEvent:n.emitEvent})}removeAt(t,e={}){let n=this._adjustIndex(t);n<0&&(n=0),this.controls[n]&&this.controls[n]._registerOnCollectionChange(()=>{}),this.controls.splice(n,1),this.updateValueAndValidity({emitEvent:e.emitEvent})}setControl(t,e,n={}){let r=this._adjustIndex(t);r<0&&(r=0),this.controls[r]&&this.controls[r]._registerOnCollectionChange(()=>{}),this.controls.splice(r,1),e&&(this.controls.splice(r,0,e),this._registerControl(e)),this.updateValueAndValidity({emitEvent:n.emitEvent}),this._onCollectionChange()}get length(){return this.controls.length}setValue(t,e={}){$e(this,!1,t),t.forEach((n,r)=>{Le(this,!1,r),this.at(r).setValue(n,{onlySelf:!0,emitEvent:e.emitEvent})}),this.updateValueAndValidity(e)}patchValue(t,e={}){t!=null&&(t.forEach((n,r)=>{this.at(r)&&this.at(r).patchValue(n,{onlySelf:!0,emitEvent:e.emitEvent})}),this.updateValueAndValidity(e))}reset(t=[],e={}){this._forEachChild((n,r)=>{n.reset(t[r],{onlySelf:!0,emitEvent:e.emitEvent})}),this._updatePristine(e,this),this._updateTouched(e,this),this.updateValueAndValidity(e)}getRawValue(){return this.controls.map(t=>t.getRawValue())}clear(t={}){this.controls.length<1||(this._forEachChild(e=>e._registerOnCollectionChange(()=>{})),this.controls.splice(0),this.updateValueAndValidity({emitEvent:t.emitEvent}))}_adjustIndex(t){return t<0?t+this.length:t}_syncPendingControls(){let t=this.controls.reduce((e,n)=>n._syncPendingControls()?!0:e,!1);return t&&this.updateValueAndValidity({onlySelf:!0}),t}_forEachChild(t){this.controls.forEach((e,n)=>{t(e,n)})}_updateValue(){this.value=this.controls.filter(t=>t.enabled||this.disabled).map(t=>t.value)}_anyControls(t){return this.controls.some(e=>e.enabled&&t(e))}_setUpControls(){this._forEachChild(t=>this._registerControl(t))}_allControlsDisabled(){for(let t of this.controls)if(t.enabled)return!1;return this.controls.length>0||this.disabled}_registerControl(t){t.setParent(this),t._registerOnCollectionChange(this._onCollectionChange)}_find(t){return this.at(t)??null}};var fi=(()=>{class i{static withConfig(e){return{ngModule:i,providers:[{provide:w,useValue:e.callSetDisabledState??Q}]}}static{this.\u0275fac=function(n){return new(n||i)}}static{this.\u0275mod=B({type:i})}static{this.\u0275inj=j({imports:[Qe]})}}return i})(),pi=(()=>{class i{static withConfig(e){return{ngModule:i,providers:[{provide:Ke,useValue:e.warnOnNgModelWithFormControl??"always"},{provide:w,useValue:e.callSetDisabledState??Q}]}}static{this.\u0275fac=function(n){return new(n||i)}}static{this.\u0275mod=B({type:i})}static{this.\u0275inj=j({imports:[Qe]})}}return i})();export{it as a,Se as b,Ve as c,ui as d,ci as e,z as f,Et as g,Ye as h,It as i,hi as j,Nt as k,xt as l,kt as m,jt as n,Ut as o,we as p,fi as q,pi as r};
