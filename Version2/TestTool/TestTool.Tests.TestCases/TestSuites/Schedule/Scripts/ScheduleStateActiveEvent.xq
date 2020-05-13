

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

declare variable $scheduleList external := ();
let $scheduleToken := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='ScheduleToken']/@Value

let $scheduleJSONList := jn:parse-json($scheduleList)
let $flag := index-of(jn:keys($scheduleJSONList), $scheduleToken) > 0
let $log := if($flag) then "Ok" else concat("Schedule list does not contain item with ScheduleToken='", $scheduleToken, "' from received notification.")

return [$flag, $log]

#####

declare variable $scheduleList external := ();
let $scheduleToken := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='ScheduleToken']/@Value
let $scheduleName := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='Name']/@Value

let $scheduleJSONList := jn:parse-json($scheduleList)
let $flag := $scheduleJSONList($scheduleToken) = $scheduleName
let $log := if($flag) then "Ok" else concat("Schedule list does not contain item with ScheduleToken='", $scheduleToken, "' and Name='", $scheduleName, "' from received notification.")

return [$flag, $log]