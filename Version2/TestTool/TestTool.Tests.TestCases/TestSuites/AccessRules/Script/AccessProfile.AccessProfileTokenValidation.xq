

let $flag := fn:exists(NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='AccessProfileToken']/@Value)
let $log := if ($flag) then "Ok" else "Received notification contains no 'AccessProfileToken' field"
return [$flag, $log]

#####

let $flag := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='AccessProfileToken']/@Value castable as tt:ReferenceToken
let $log := if ($flag) then "Ok" else "Received notification contains 'AccessProfileToken' field, but its type isn't tt:ReferenceToken"
return [$flag, $log]

#####

declare variable $accessProfileList external := ();
let $accessProfileToken := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='AccessProfileToken']/@Value

let $accessProfileJSONList := jn:parse-json($accessProfileList)
let $flag := index-of(jn:keys($accessProfileJSONList), $accessProfileToken) > 0
let $log := if($flag) then "Ok" else concat("Received notification does not contain item with AccessProfileToken='", $accessProfileToken, "'")

return [$flag, $log]