let $flag := fn:exists(NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='Token']/@Value)
let $log := if ($flag) then "Ok" else "Received notification contains no 'Token' field"
return [$flag, $log]

#####

let $flag := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='Token']/@Value castable as tt:ReferenceToken
let $log := if ($flag) then "Ok" else "Received notification contains 'Token' field, but its type isn't tt:ReferenceToken"
return [$flag, $log]

#####

let $flag := fn:exists(NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='Value']/@Value)
let $log := if ($flag) then "Ok" else "Received notification contains no 'Value' field"
return [$flag, $log]

#####

let $flag := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='Value']/@Value castable as xs:float
let $log := if ($flag) then "Ok" else "Received notification contains 'Value' field, but its type isn't xs:float"
return [$flag, $log]

#####

let $floatValue := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Data/tt:SimpleItem[@Name='Value']/@Value cast as xs:float
let $flag := (0 <= $floatValue) and ($floatValue <= 100)
let $log := if ($flag) then "Ok" else "Received notification contains 'Value' field of type xs:float, but its value is out of range [0, 100]"
return [$flag, $log]
