let $flag := fn:exists(NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='Token']/@Value)
let $log := if ($flag) then "Ok" else "Received notification contains no 'Token' field"
return [$flag, $log]

#####

let $flag := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='Token']/@Value castable as tt:ReferenceToken
let $log := if ($flag) then "Ok" else "Received notification contains 'Token' field, but its type isn't tt:ReferenceToken"
return [$flag, $log]

#####

let $flag := fn:exists(NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='Failed']/@Value)
let $log := if ($flag) then "Ok" else "Received notification contains no 'Failed' field"
return [$flag, $log]

#####

let $flag := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='Failed']/@Value castable as xs:boolean
let $log := if ($flag) then "Ok" else "Received notification contains 'Failed' field, but its type isn't xs:boolean"
return [$flag, $log]
