

let $flag := fn:exists(NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='CredentialToken']/@Value)
let $log := if ($flag) then "Ok" else "Received notification contains no 'CredentialToken' field"
return [$flag, $log]

#####

let $flag := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='CredentialToken']/@Value castable as tt:ReferenceToken
let $log := if ($flag) then "Ok" else "Received notification contains 'CredentialToken' field, but its type isn't tt:ReferenceToken"
return [$flag, $log]

#####

declare variable $credentialList external := ();
let $credentialToken := NotificationMessageHolderType/wsnt:Message/tt:Message/tt:Source/tt:SimpleItem[@Name='CredentialToken']/@Value

let $credentialJSONList := jn:parse-json($credentialList)
let $flag := index-of(jn:keys($credentialJSONList), $credentialToken) > 0
let $log := if($flag) then "Ok" else concat("Received notification does not contain item with CredentialToken='", $credentialToken, "'")

return [$flag, $log]