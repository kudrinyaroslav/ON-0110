let $flag := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='Status']/@Value castable as xs:dateTime
let $log := if ($flag) then "Ok" else "Received notification contains no 'Status' field of type xs:dateTime"
return [$flag, $log]

#####

let $flag := fn:matches(NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='Status']/@Value, '.*T[^+\-]*Z')
let $log := if ($flag) then "Ok" else "Received notification contains 'Status' field of type xs:dateTime but its value isn't in UTC format"
return [$flag, $log]

#####

let $flag := (NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='Status']/@Value cast as xs:dateTime) >= xs:dateTime('0001-01-01T00:00:00Z')
return [$flag, if ($flag) then "Ok" else "Received notification contains 'Status' field of type xs:dateTime in UTC format but its value is earlier than '0001-01-01T00:00:00Z'"]
