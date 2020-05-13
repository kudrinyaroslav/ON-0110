let $flag := fn:exists(NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='Critical']/@Value)
let $log := if ($flag) then "Ok" else "Received notification contains no 'Critical' field"
return [$flag, $log]

#####

let $flag := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='Critical']/@Value castable as xs:boolean
let $log := if ($flag) then "Ok" else "Received notification contains 'Critical' field, but its type isn't xs:boolean"
return [$flag, $log]
