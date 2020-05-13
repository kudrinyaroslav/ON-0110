

let $flag := fn:exists(NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='ScheduleToken']/@Value)
let $log := if ($flag) then "Ok" else "Received notification contains no 'ScheduleToken' field"
return [$flag, $log]

#####

let $flag := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='ScheduleToken']/@Value castable as tt:ReferenceToken
let $log := if ($flag) then "Ok" else "Received notification contains 'ScheduleToken' field, but its type isn't tt:ReferenceToken"
return [$flag, $log]

#####

let $flag := fn:exists(NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='Name']/@Value)
let $log := if ($flag) then "Ok" else "Received notification contains no 'Name' field"
return [$flag, $log]

#####

let $flag := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='Name']/@Value castable as xs:string
let $log := if ($flag) then "Ok" else "Received notification contains 'Name' field, but its type isn't xs:string"
return [$flag, $log]

#####

let $flag := fn:exists(NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='Active']/@Value)
let $log := if ($flag) then "Ok" else "Received notification contains no 'Active' field"
return [$flag, $log]

#####

let $flag := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='Active']/@Value castable as xs:boolean
let $log := if ($flag) then "Ok" else "Received notification contains 'Active' field, but its type isn't xs:boolean"
return [$flag, $log]

#####

let $flag := fn:exists(NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='SpecialDay']/@Value)
let $log := if ($flag) then "Ok" else "Received notification contains no 'SpecialDay' field"
return [$flag, $log]

#####

let $flag := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='SpecialDay']/@Value castable as xs:boolean
let $log := if ($flag) then "Ok" else "Received notification contains 'SpecialDay' field, but its type isn't xs:boolean"
return [$flag, $log]

#####

declare variable $scheduleNameExpected external := ();
declare variable $scheduleToken external := ();

let $scheduleName := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='Name']/@Value

let $isScheduleNameCorrect := $scheduleNameExpected = $scheduleName
let $log := if($isScheduleNameCorrect) then "Ok" else concat("Receiving Schedule notification item with ScheduleToken='", $scheduleToken, "' Name value is '", $scheduleName, "'. Expected value is '", $scheduleNameExpected)

return [$isScheduleNameCorrect, $log]

#####

declare variable $activeExpected external := ();
declare variable $scheduleToken external := ();

let $active := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='Active']/@Value cast as xs:boolean

let $isActiveCorrect := $activeExpected cast as xs:boolean = $active
let $log := if($isActiveCorrect) then "Ok" else concat("Receiving Schedule notification item with ScheduleToken='", $scheduleToken, "' Active value is ", $active, ". Expected value is ", $activeExpected)

return [$isActiveCorrect, $log]

#####

declare variable $specialDayExpected external := ();
declare variable $scheduleToken external := ();

let $specialDay := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='SpecialDay']/@Value cast as xs:boolean

let $isSpecialDayCorrect := $specialDayExpected cast as xs:boolean = $specialDay
let $log := if($isSpecialDayCorrect) then "Ok" else concat("Receiving Schedule notification item with ScheduleToken='", $scheduleToken, "' SpecialDay value is ", $specialDay, ". Expected value is ", $specialDayExpected)

return [$isSpecialDayCorrect, $log]