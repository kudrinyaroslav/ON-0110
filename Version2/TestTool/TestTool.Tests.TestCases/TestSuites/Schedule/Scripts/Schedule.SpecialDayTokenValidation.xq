

let $flag := fn:exists(NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='SpecialDaysToken']/@Value)
let $log := if ($flag) then "Ok" else "Received notification contains no 'SpecialDaysToken' field"
return [$flag, $log]

#####

let $flag := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='SpecialDaysToken']/@Value castable as tt:ReferenceToken
let $log := if ($flag) then "Ok" else "Received notification contains 'SpecialDaysToken' field, but its type isn't tt:ReferenceToken"
return [$flag, $log]

#####

declare variable $specialDayGroups external := ();
let $specialDaysToken := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='SpecialDaysToken']/@Value

let $specialDayGroupJSONList := jn:parse-json($specialDayGroups)
let $flag := index-of(jn:keys($specialDayGroupJSONList), $specialDaysToken) > 0
let $log := if($flag) then "Ok" else concat("Received notification does not contain item with SpecialDaysToken='", $specialDaysToken, "'")

return [$flag, $log]