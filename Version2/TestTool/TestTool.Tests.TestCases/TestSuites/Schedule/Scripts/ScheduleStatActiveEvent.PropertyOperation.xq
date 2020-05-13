

let $flag := fn:exists(NotificationMessageHolderType/wsnt:Message/tt:Message[@PropertyOperation='Initialized'])
let $log := if ($flag) then "Ok" else "Received notification contains no 'PropertyOperation' field"
return [$flag, $log]