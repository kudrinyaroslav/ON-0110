let $flag := fn:exists(NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='Status']/@Value)
let $log := if ($flag) then "Ok" else "Received notification contains no 'Status' field"
return [$flag, $log]

#####

let $flag := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='Status']/@Value castable as xs:dateTime
let $log := if ($flag) then "Ok" else "Received notification contains 'Status' field, but its type isn't xs:dateTime"
return [$flag, $log]
