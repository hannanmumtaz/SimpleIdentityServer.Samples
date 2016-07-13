﻿using System;
using System.Net.Http;
using System.Text;
using System.Xml;
using WsFederation.Retriever;

namespace WsFederation.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var inf = "%3C%3Fxml+version%3D%221.0%22+encoding%3D%22UTF-8%22%3F%3E%3Cwst%3ARequestSecurityTokenResponseCollection+xmlns%3Awst%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fws-sx%2Fws-trust%2F200512%22+xmlns%3Ads%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2F09%2Fxmldsig%23%22+xmlns%3Amd%3D%22urn%3Aoasis%3Anames%3Atc%3ASAML%3A2.0%3Ametadata%22+xmlns%3Asaml2%3D%22urn%3Aoasis%3Anames%3Atc%3ASAML%3A2.0%3Aassertion%22+xmlns%3Awsa%3D%22http%3A%2F%2Fwww.w3.org%2F2005%2F08%2Faddressing%22+xmlns%3Awsse%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2F2004%2F01%2Foasis-200401-wss-wssecurity-secext-1.0.xsd%22+xmlns%3Awsu%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2F2004%2F01%2Foasis-200401-wss-wssecurity-utility-1.0.xsd%22+xmlns%3Axenc%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2F04%2Fxmlenc%23%22%3E%3Cwst%3ARequestSecurityTokenResponse%3E%3Cwst%3ATokenType%3Eurn%3Aoasis%3Anames%3Atc%3ASAML%3A2.0%3Aassertion%3C%2Fwst%3ATokenType%3E%3Cwst%3ARequestType%3Ehttp%3A%2F%2Fdocs.oasis-open.org%2Fws-sx%2Fws-trust%2F200512%2FIssue%3C%2Fwst%3ARequestType%3E%3Cwst%3AKeyType%3Ehttp%3A%2F%2Fdocs.oasis-open.org%2Fws-sx%2Fws-trust%2F200512%2FBearer%3C%2Fwst%3AKeyType%3E%3Cwst%3ARequestedSecurityToken%3E%3Csaml2%3AAssertion+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+ID%3D%22assertion-18cb001b-2d4b-467f-a680-d2cc32d4e0f8%22+IssueInstant%3D%222016-07-13T14%3A04%3A38.645Z%22+Version%3D%222.0%22+xmlns%3Asaml2%3D%22urn%3Aoasis%3Anames%3Atc%3ASAML%3A2.0%3Aassertion%22%3E%3Csaml2%3AIssuer%3Ewww.e-contract.be%3C%2Fsaml2%3AIssuer%3E%3Cds%3ASignature+xmlns%3Ads%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2F09%2Fxmldsig%23%22%3E%3Cds%3ASignedInfo%3E%3Cds%3ACanonicalizationMethod+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2F10%2Fxml-exc-c14n%23%22%2F%3E%3Cds%3ASignatureMethod+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2F09%2Fxmldsig%23rsa-sha1%22%2F%3E%3Cds%3AReference+URI%3D%22%23assertion-18cb001b-2d4b-467f-a680-d2cc32d4e0f8%22%3E%3Cds%3ATransforms%3E%3Cds%3ATransform+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2F09%2Fxmldsig%23enveloped-signature%22%2F%3E%3Cds%3ATransform+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2F10%2Fxml-exc-c14n%23%22%2F%3E%3C%2Fds%3ATransforms%3E%3Cds%3ADigestMethod+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2F09%2Fxmldsig%23sha1%22%2F%3E%3Cds%3ADigestValue%3EjNlraCbAvRwdLax2ewC%2FYRVPag0%3D%3C%2Fds%3ADigestValue%3E%3C%2Fds%3AReference%3E%3C%2Fds%3ASignedInfo%3E%3Cds%3ASignatureValue%3EVkDgnGqya2xoEZYT7bp431%2F%2BFg36ZBzHuJWDBhXlZOJO2nE6fI%2BofLg2GKBVasx%2FLFRG%2FsFI1m6tJFishx5MgNG61HWBTNVFVYRt8xMArONIUakXGsdXLpKbjyPXQkdfWBh9NsY%2BB8FFVSAohRQ9M%2FaUZ%2FyLEK745eAiTi8LAG4zxEVg%2FnmUS%2FhbYMel5YeT%2BeHRKkbOSSoLNOplUE5I0Y7bv3LESmtyhoR3yInu0QbXap9Ev%2FuhUPMetjdlBOzhZfohq5PGhtXi4zk2hCq%2F3NALyJLsqP7EfdKP1OF%2BBHxxb7yI%2BK61i1aslv7SrVGz7wvB861L%2BNTKQMGK9JWv4w%3D%3D%3C%2Fds%3ASignatureValue%3E%3Cds%3AKeyInfo%3E%3Cds%3AX509Data%3E%3Cds%3AX509Certificate%3EMIIFLTCCBBWgAwIBAgIHS2X8ooaaSzANBgkqhkiG9w0BAQsFADCBtDELMAkGA1UEBhMCVVMxEDAOBgNVBAgTB0FyaXpvbmExEzARBgNVBAcTClNjb3R0c2RhbGUxGjAYBgNVBAoTEUdvRGFkZHkuY29tLCBJbmMuMS0wKwYDVQQLEyRodHRwOi8vY2VydHMuZ29kYWRkeS5jb20vcmVwb3NpdG9yeS8xMzAxBgNVBAMTKkdvIERhZGR5IFNlY3VyZSBDZXJ0aWZpY2F0ZSBBdXRob3JpdHkgLSBHMjAeFw0xNDA0MTEwNDI4MzJaFw0xNzA0MTEwNDI4MzJaMD8xITAfBgNVBAsTGERvbWFpbiBDb250cm9sIFZhbGlkYXRlZDEaMBgGA1UEAxMRd3d3LmUtY29udHJhY3QuYmUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQC4KtCOOfoeW4HxyKxjRNf0DtXYyI60D%2FCrB3zoiWlvbNhbQuQeui5HSzDbo90FgOMQE7OGlaC%2BTInePOVlmPaMjBn1ZvkmI8q1Y2QBu3Rf8gEIMFQorsPKUx69YiUEy94sguSFPmxvCevbREHhPvAL6xfAEZuJyGlGIppkTOZ%2BAIT8%2B1HpT5MFX78CxwbIm5GR6mzMCONk%2BgEd1GdcSOBsXckWDmMVEKoat6OKYF%2FtwuP2cHqqD9WXOkUL347n7q5jUbbuJlcMa1Io9GNoLK5RYMWhIMjqp4gLT2xg2QVd6kqtQX9xzvypYn7wpkysg4zZ3hRr1sCU18nHsBreuR6%2FAgMBAAGjggG2MIIBsjAPBgNVHRMBAf8EBTADAQEAMB0GA1UdJQQWMBQGCCsGAQUFBwMBBggrBgEFBQcDAjAOBgNVHQ8BAf8EBAMCBaAwNgYDVR0fBC8wLTAroCmgJ4YlaHR0cDovL2NybC5nb2RhZGR5LmNvbS9nZGlnMnMxLTQwLmNybDBTBgNVHSAETDBKMEgGC2CGSAGG%2FW0BBxcBMDkwNwYIKwYBBQUHAgEWK2h0dHA6Ly9jZXJ0aWZpY2F0ZXMuZ29kYWRkeS5jb20vcmVwb3NpdG9yeS8wdgYIKwYBBQUHAQEEajBoMCQGCCsGAQUFBzABhhhodHRwOi8vb2NzcC5nb2RhZGR5LmNvbS8wQAYIKwYBBQUHMAKGNGh0dHA6Ly9jZXJ0aWZpY2F0ZXMuZ29kYWRkeS5jb20vcmVwb3NpdG9yeS9nZGlnMi5jcnQwHwYDVR0jBBgwFoAUQMK9J47MNIMwojPX%2B2yz8LQsgM4wKwYDVR0RBCQwIoIRd3d3LmUtY29udHJhY3QuYmWCDWUtY29udHJhY3QuYmUwHQYDVR0OBBYEFKlGMfaulaBUuzpcxgjLT5hJtJjaMA0GCSqGSIb3DQEBCwUAA4IBAQClS6PGpIgNL4OTAxfi3TNnplf6KdJFqMgeYFot9gGrJui2Mw6LI6DrVNscrlqEKvoluUYLRTS3M0b1DPT7t3pniOkDVWj%2FhYwjapIePVwcYq3j%2FSpVwd0IPbD%2BtDcvYhZya2GWLlDbxREG09677U6pQUomQCpkAyL6%2F9rd8nZIp5sjEAn06U50L1tQdHYbYp11pyJgaIYy%2B%2FcnPhoPuYxddZj5U1tU0F6ZirlF0RheRiY48ZsRBNM2lfl%2BInsxwmrSI%2FdZ%2BzqD2S395obTtiI%2BSG6irUHcHOY1P45fmqXDXDWzx9ES%2FWN5hy5Kyeypjp3k4gJbKzq9FWI2ufX11759%3C%2Fds%3AX509Certificate%3E%3C%2Fds%3AX509Data%3E%3C%2Fds%3AKeyInfo%3E%3C%2Fds%3ASignature%3E%3Csaml2%3ASubject%3E%3Csaml2%3ANameID%3E89100739573%3C%2Fsaml2%3ANameID%3E%3Csaml2%3ASubjectConfirmation+Method%3D%22urn%3Aoasis%3Anames%3Atc%3ASAML%3A2.0%3Acm%3Abearer%22%2F%3E%3C%2Fsaml2%3ASubject%3E%3Csaml2%3AConditions+NotBefore%3D%222016-07-13T14%3A04%3A38.645Z%22+NotOnOrAfter%3D%222016-07-13T14%3A09%3A38.645Z%22%3E%3Csaml2%3AAudienceRestriction%3E%3Csaml2%3AAudience%3Ehttp%3A%2F%2Flocalhost%3A5101%2Fapi%2Fclients%3C%2Fsaml2%3AAudience%3E%3C%2Fsaml2%3AAudienceRestriction%3E%3C%2Fsaml2%3AConditions%3E%3Csaml2%3AAttributeStatement%3E%3Csaml2%3AAttribute+Name%3D%22be%3Afedict%3Aeid%3Aidp%3Adob%3Ayear%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Astring%22%3E1989%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22be%3Afedict%3Aeid%3Aidp%3Adob%3Amonth%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Astring%22%3E10%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22be%3Afedict%3Aeid%3Aidp%3Acard-validity%3Abegin%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3AdateTime%22%3E2014-01-30T00%3A00%3A00Z%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Flocality%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Astring%22%3EBruxelles%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22be%3Afedict%3Aeid%3Aidp%3Acard-validity%3Aend%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3AdateTime%22%3E2019-01-30T00%3A00%3A00Z%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22be%3Afedict%3Aeid%3Aidp%3Acard-number%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Astring%22%3EB174798747%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Fprivatepersonalidentifier%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Astring%22%3E89100739573%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22be%3Afedict%3Aeid%3Aidp%3Anationality%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Astring%22%3EFrance%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22be%3Afedict%3Aeid%3Aidp%3Acert%3Aauthn%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Abase64Binary%22%3EMIID7zCCAtegAwIBAgIQEAAAAAAAjzo9FBi9BAvSgTANBgkqhkiG9w0BAQUFADA1MQswCQYDVQQGEwJCRTEVMBMGA1UEAxMMRm9yZWlnbmVyIENBMQ8wDQYDVQQFEwYyMDE0MDIwHhcNMTQwMjA1MTIzMzEyWhcNMTkwMTMwMjM1OTU5WjB3MQswCQYDVQQGEwJGUjEoMCYGA1UEAxMfVGhpZXJyeSBIYWJhcnQgKEF1dGhlbnRpY2F0aW9uKTEPMA0GA1UEBBMGSGFiYXJ0MRcwFQYDVQQqEw5UaGllcnJ5IFJvYmVydDEUMBIGA1UEBRMLODkxMDA3Mzk1NzMwgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBAI4At%2F6KxOpRKiFEluuMVRaP%2BYI4gKKeVpl10CY%2BvPln9P3dvUh7gdNWfEN2Fn0LzvOslVDWZeb%2BA8%2FHBhPePpTJ9sACPJG%2BBjcLKZYKwjc6eEKAHmDTqClTuvrSi5hzpEuDJ7gHQmzJcochdcKRr06MIg3wP7P8s91VxSJoKJ0%2FAgMBAAGjggE7MIIBNzAfBgNVHSMEGDAWgBTD%2FrhgVgSUYRHFYhTWJlYgS27SvTBwBggrBgEFBQcBAQRkMGIwNgYIKwYBBQUHMAKGKmh0dHA6Ly9jZXJ0cy5laWQuYmVsZ2l1bS5iZS9iZWxnaXVtcnMyLmNydDAoBggrBgEFBQcwAYYcaHR0cDovL29jc3AuZWlkLmJlbGdpdW0uYmUvMjBEBgNVHSAEPTA7MDkGB2A4CQEBBwIwLjAsBggrBgEFBQcCARYgaHR0cDovL3JlcG9zaXRvcnkuZWlkLmJlbGdpdW0uYmUwOQYDVR0fBDIwMDAuoCygKoYoaHR0cDovL2NybC5laWQuYmVsZ2l1bS5iZS9laWRmMjAxNDAyLmNybDAOBgNVHQ8BAf8EBAMCB4AwEQYJYIZIAYb4QgEBBAQDAgWgMA0GCSqGSIb3DQEBBQUAA4IBAQBSz7layb5HgSQM%2BZB2np1%2FgAUJ%2B8wuxoU62vxGrj7T1K5tmRK5WeX4rpEJ8h0Nv1169GGng77xlAdE9s8noQXQ063SuBg%2FQAXSYTRF2T%2Bg9rZinC12aljo%2FbLz8jWSBY8AM4GZQcVAQ31aD%2BmWzO0DPwVtDvhl0%2F6jFVprKFDhAnPJvzMQe7a6zwi5lL%2BGXIKrnNhlvfQDtxJRV4Jr1vps7ZBjXnAOuatMIxlJ9eafU3dgQ2TvTpwSVDV8UgrInhKD9cLBzymli4LeMWyD6qC3SwfDwRVODqNSIogBnSH91fn3nZUDKpUYfOCSGxNozGYZsVnqwO0jJf4E69O5lUgg%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Fdateofbirth%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3AdateTime%22%3E1989-10-07T00%3A00%3A00Z%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Fgender%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Astring%22%3E1%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22be%3Afedict%3Aeid%3Aidp%3Apob%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Astring%22%3ERambouillet%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Fgivenname%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Astring%22%3EThierry+Robert%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22be%3Afedict%3Aeid%3Aidp%3Aphoto%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Abase64Binary%22%3E%2F9j%2F4AAQSkZJRgABAgEBLAEsAAD%2F2wBDAA4KCw0LCQ4NDA0QDw4RFiQXFhQUFiwgIRokNC43NjMuMjI6QVNGOj1OPjIySGJJTlZYXV5dOEVmbWVabFNbXVn%2FwAALCADIAIwBAREA%2F8QA0gAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoLEAACAQMDAgQDBQUEBAAAAX0BAgMABBEFEiExQQYTUWEHInEUMoGRoQgjQrHBFVLR8CQzYnKCCQoWFxgZGiUmJygpKjQ1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4eLj5OXm5%2Bjp6vHy8%2FT19vf4%2Bfr%2F2gAIAQEAAD8A9JoooooprusaF3YKoGSScAVk3HibSrclWugzDsqk1FH4r0l%2BtwV%2BqmrUOvaZOwVLtMn1BH8xWhHIkiho3V1PdTmn0UUUUUUUUUhOBzWNqniO0sAyg%2BZKONo7fWsCXxpKclIkAx3zXP6nr97fKQ8zBD2BOKw5ZSBycn1NVhOd33uKvW12y8iQke5rZttWngAMMzp7KSK2bPxZeIAGKy%2Bziuo0%2FXrO8WNWkEUzfwN6%2BxrWooooooormPE%2FiGOyjNtbvmduCR%2FDXn088kjlmcliecmq7SdixyahmkKLz%2FOqTuW5z%2BNMxnG2lV2DYPSrsE4PBcgVoQv6OGqUO6ncGJH6ius8P%2BKDFtt71zJF0WQ%2FeX6%2BtdqjK6BlOVYZBHenUUUUVma9qkelac8zEeY3Ea56mvKLu4kmmeaRy0jknk9KrA4yXIzUMswGQoG719KoyOxPJ%2FGmA44zRnGOakUknIXP4VaUKw4XDelSRzhHxgirnmkJuX5h35pUcH5l4PqK7fwbruR9gun9BCxP%2Fjv%2BFdpRRRSV5r4w1UXl8yJ%2Fq4SUXnIznk1yRbJLHNA%2BYelNaItnFRNbMT6D6UfZcDpmhbVhj5cn6VJ9mlC52kUqpIvYnHrUyFWG1hwe%2FcVKsewgK2QaD8j8ADPapYrh4mDoSGU5GOor1Lw1qo1XTFZj%2B%2BiAWTnJPv8AjWxRRWF4t1M6bo7%2BW4Wab5E55Hqfyryu4dmAz3qqzbRyKdEjyHvWhBYSSEcN%2BVacOkjHz5x7Cpv7JEh%2BVAqj1HNaFvoiKvzLn8KdJosbJjGPpWZdaI0akpyPpWNPZtGTkEEVXDPGdh5FPLFlx1I6VGsnfuOCK3PD2ryaVqMcin9xKQko9s9fwr1ZSGUEHIPINLRXmPju9N1rZt1b5LdAv4nk%2FwBK5ZnJP06UkEEsz8KSTXT6ZoxChpAOa6CDT0RRtAFWUsVLZY5qcWqJ90U4L2zSMhGO9Ryxhhjisq9slcH5RXNahYlGJA4rLOVY5qBm2yn0NTocjb616p4PvTeaDCJG3Sw5jb8On6YrepK8b19i2s3%2BeSZ35%2F4EazkUFsV02jWi7AxHNdDCABxxVpMipw1KWwM9aTeMdDSFgaiY4HWq8mGzWbeQB1bjmubvbMqu4CsqVfwxSRtgivQvh%2FIQbuPJ2kK2PzrtqQ9DXiurHGqXQzn96%2FJ78mqcLfPk12Ok8QKcdRWvGanVgKmBBFOAJHBowc4o781FIOOOlV3IqCTBFULqJTGc1yV4AkxHoeKqg%2FPXd%2FDtz9rulOf9WP5139JXjXiWEwa%2FfKcY81iPoTn%2BtZVuS8oUd67G3lW3tlLEAKKqSeItrkRjgd6VPEbnHGfwrUstdimYKdwP0rYiuAwyDwakMuDUb3AUc1n3GpKgYnoKyX16EtjcfyqRNYgYg7qneVJ48oQRXKawpinz2NUIzmT1rtfAcjLqpC5IKENXpFFeZ%2BP7PydY89VO2ZASffkf4VzGlRiTUkGOn%2BFal3HJcy%2BWDtReKE0y0iizM5J%2BtVzDYk%2FI7D3FSw2y5DRSgkVuWFyyja55FaySFkzVC%2FlZV64rnrl5Z8heAao%2F2c5bJkUVIumOB8sgNLC9zZS%2Bq96m1MC5s%2FNHUc1kWy8k16J8PbZfJubgjnIUGu2ork%2FHLxPax25QNKfmBPYVwehxZ1F29Aa22gYFmHesm4heS4VWJ2k4z6VT1KxmtrkBD8rD5Secip4ba4itFk2uzE8D2rSsvOwrOhX1BrpYACg5rO1ddsJx1rkpWut4Hltt7kdqr3kNwko2ZwRnk1dt7eZbMTb2BzwPWr9sGmT515qW5h22Uox%2FCawraIkhcHk4P0r2Dw%2FbwW2j26WxJj25yeuc8%2FrWnRXEeMEL6mg%2F6ZgD8zXOaPH5eo3APbiujEYaPFNS0j9Oaf8AY1Bzz%2BdNaH0prIOhqzCxCdBVK%2BG%2FANRxRBuMVOYInXaRQbCIjgEikFoqEYFVr%2BICJwP7prnreLMRZQcg16j4aBXQrYH0P%2FoRrVorlvFEH%2BmQy44K4%2FEZrkLNsaxKBwCDXRwtleanG0UZ4IpGAVSaqht77RVuJN1QXMYbjGCKqwHD4PUVeCA9KcCBxikYgDNZt8wMb%2FQ1laYgaF1IyK9J0eIw6VbowwQucfU5q9RWT4ihEmnb%2B8bA5%2FSuCitJY9TErDCnPPrWzE2B61Ojc8mplximSn5TVETxw5ZmHNXIL5AMqRn3qpeahGgyzDJqCzkEswbPWtgAY4qKTiq7Px14qhet%2B4kI9DTdBti80UbjHmuor0kAAADgCloqtqEH2mxmiHVl4%2FnXByfu3CZ796mjNWU6D1qTdgVHKcqcVz2oRyOSjRF1zxUUPmRKFyQvoe1RXcUkx5yyitLSUkWRSwwo6V0Afio3OTVWWqlxzE3Q54rZ8L2ayXnnkf6tfyNdfRRRXK6xo04nMlvH5qMS2FHK%2B1YykqcEYxVlG4GKdu4NRu%2FUVGF3jkfnTLq1Bj3BM%2FSo0gAi5XGfWhW8k4GcVaSfjrTjICeDUMj5PWiCwuL%2FAHLapvKkFuQMV2Gj6f8A2fa7WIMr4LY7e1aNFFFFcHqK%2BXqFyAMASNx%2BNRRNk9ae8mOBioXlRMszAD3qnLqyKdqEfWq%2F9pHOfNI%2BpqI6i7NnezfTJqUamOkvHv0qxDdIwyrAj2qwsu4ZzSlq6XwkhEVy%2FZioH4Z%2FxroqKKKKK47xEqrqkhA6gE%2FXFZK8HiiQEjiq81ibhfnc49qr%2FwBnCFvlUHHc0u1QCHjU%2FWmNtxgKBSfYxIMtgirEFlCg%2BQbTU%2B0RjANMz3Nd3oFv9n0qLI%2BaT5z%2BP%2F1q06KKKKK43xIwGqv%2FALo%2FlWQDk8Gl8wZGalVgR60MciqF1tGQRn6CoIkAbOw%2FlVsAY9qDIRwKaWyeTViyhFzewwk4DuFJFejIixoqKMKowB7U6iiiiiuJ8T5%2FtWTnsP5VirLtOG6U4kEZBqSN%2BcEVMnzcDpQ0Kk%2FMoNK0SoMgAVXlwvQ%2FhVRnHJBOaaHGM55rT0P5tXtB%2FwBNAa9FoooooqpqOo22m2zT3UgRQOB3b2A71xF9fjU5TdBdofoPas9lDA5quztFlT3p0VyOlWI7kZ7VaEo5yc015vlzzWdPL1xVGW4O7ANTREnlutX7C%2FGnXUd2ybxEc7c4zxXaaf4t0m%2FYIJjA7YAWYBc%2Fj0rd60tFZera%2Fp2kL%2FpU48ztEnzOfw%2Fxrh9W%2BId1KzR6dElvGRgO43P%2FAID9a5C4vpbh2klkaSRySWY5JrprE%2F6BCP8AYH8qmxUUyBl5rPmieM5TJqEXLKeRyPWpTfN%2FdNNN82OetV3meTO0H61LDFjk8mrK1Dey7baQEdRWFHOQCOo9K6fw%2FwCLb7SwsAKzW3HySZO0f7J7V6Lo3iGz1dcRkxTD%2Flm5GT9PWtivnuWfcTz1qEtkfWlJxXYaW%2B60hI%2FuD%2BVXWU9R0NNIyKrSKR2qMxo3BAppt48%2FdprRRr0AzUO3PWpAuBmgEVnatJthx3JrDVsGrcR3Jx17VYguXibAbj0retvF%2BpWsIhjvHCL0DKGx%2BJGa4wnJ70ZpcnjFdT4en8202E%2FNGcfhW6DlelNIAHFROAetNKA0wpj6VC6eopm2gj3qvPKkCFnOAK5%2B9umuHJ%2Fh7VUFSRttbrUwYngUwk5Nf%2F%2FZ%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Fstreetaddress%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Astring%22%3EAvenue+des+Croix+du+Feu+223+%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22be%3Ae-contract%3Aeid%3Aidp%3Adocument-type%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Astring%22%3EFOREIGNER_E%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Fname%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Astring%22%3EThierry+Robert+Habart%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Fsurname%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Astring%22%3EHabart%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22be%3Afedict%3Aeid%3Aidp%3Adob%3Aday%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Astring%22%3E7%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22be%3Afedict%3Aeid%3Aidp%3Aage%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Ainteger%22%3E26%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3Csaml2%3AAttribute+Name%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Fpostalcode%22%3E%3Csaml2%3AAttributeValue+xmlns%3Axs%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%22+xmlns%3Axsi%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema-instance%22+xsi%3Atype%3D%22xs%3Astring%22%3E1020%3C%2Fsaml2%3AAttributeValue%3E%3C%2Fsaml2%3AAttribute%3E%3C%2Fsaml2%3AAttributeStatement%3E%3Csaml2%3AAuthnStatement+AuthnInstant%3D%222016-07-13T14%3A04%3A38.645Z%22%3E%3Csaml2%3AAuthnContext%3E%3Csaml2%3AAuthnContextClassRef%3Eurn%3Abe%3Afedict%3Aeid%3Aidp%3AAuthenticationWithIdentification%3C%2Fsaml2%3AAuthnContextClassRef%3E%3C%2Fsaml2%3AAuthnContext%3E%3C%2Fsaml2%3AAuthnStatement%3E%3C%2Fsaml2%3AAssertion%3E%3C%2Fwst%3ARequestedSecurityToken%3E%3C%2Fwst%3ARequestSecurityTokenResponse%3E%3C%2Fwst%3ARequestSecurityTokenResponseCollection%3E";
            // var d = new XmlDocument();
            // d.LoadXml(inf);
            var decoded = Convert.FromBase64String(inf);
            System.Console.WriteLine(decoded);
            System.Console.ReadLine();
            /*
            var httpClient = new HttpClient();
            WsFedMetadataRetriever.GetFederationMetataData("https://www.e-contract.be/eid-idp/endpoints/ws-federation/metadata/auth-ident-metadata.xml", httpClient);
            System.Console.ReadLine();
            */
        }
    }
}
