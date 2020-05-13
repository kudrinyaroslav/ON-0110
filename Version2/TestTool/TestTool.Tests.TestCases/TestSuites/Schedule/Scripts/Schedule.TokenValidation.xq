

let $flag := fn:exists(NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='ScheduleToken']/@Value)
let $log := if ($flag) then "Ok" else "Received notification contains no 'ScheduleToken' field"
return [$flag, $log]

#####

let $flag := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='ScheduleToken']/@Value castable as tt:ReferenceToken
let $log := if ($flag) then "Ok" else "Received notification contains 'ScheduleToken' field, but its type isn't tt:ReferenceToken"
return [$flag, $log]

#####

declare variable $scheduleList external := ();
let $scheduleToken := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='ScheduleToken']/@Value

let $scheduleJSONList := jn:parse-json($scheduleList)
let $flag := index-of(jn:keys($scheduleJSONList), $scheduleToken) > 0
let $log := if($flag) then "Ok" else concat("Received notification does not contain item with ScheduleToken='", $scheduleToken, "'")

return [$flag, $log]