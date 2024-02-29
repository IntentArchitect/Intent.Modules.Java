using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Types.Api;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModelExtensions", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Api
{
    public static class TypeDefinitionModelStereotypeExtensions
    {
        public static CheckedExceptionHandling GetCheckedExceptionHandling(this TypeDefinitionModel model)
        {
            var stereotype = model.GetStereotype("05d3f924-e9c7-4ce1-b8f4-f29bc8e7993e");
            return stereotype != null ? new CheckedExceptionHandling(stereotype) : null;
        }


        public static bool HasCheckedExceptionHandling(this TypeDefinitionModel model)
        {
            return model.HasStereotype("05d3f924-e9c7-4ce1-b8f4-f29bc8e7993e");
        }

        public static bool TryGetCheckedExceptionHandling(this TypeDefinitionModel model, out CheckedExceptionHandling stereotype)
        {
            if (!HasCheckedExceptionHandling(model))
            {
                stereotype = null;
                return false;
            }

            stereotype = new CheckedExceptionHandling(model.GetStereotype("05d3f924-e9c7-4ce1-b8f4-f29bc8e7993e"));
            return true;
        }

        public class CheckedExceptionHandling
        {
            private IStereotype _stereotype;

            public CheckedExceptionHandling(IStereotype stereotype)
            {
                _stereotype = stereotype;
            }

            public string Name => _stereotype.Name;

            public HttpResponseStatusOptions HttpResponseStatus()
            {
                return new HttpResponseStatusOptions(_stereotype.GetProperty<string>("Http Response Status"));
            }

            public bool Log()
            {
                return _stereotype.GetProperty<bool>("Log");
            }

            public class HttpResponseStatusOptions
            {
                public readonly string Value;

                public HttpResponseStatusOptions(string value)
                {
                    Value = value;
                }

                public HttpResponseStatusOptionsEnum AsEnum()
                {
                    switch (Value)
                    {
                        case "CONTINUE (100)":
                            return HttpResponseStatusOptionsEnum.CONTINUE100;
                        case "SWITCHING_PROTOCOLS (101)":
                            return HttpResponseStatusOptionsEnum.SWITCHING_PROTOCOLS101;
                        case "PROCESSING (102)":
                            return HttpResponseStatusOptionsEnum.PROCESSING102;
                        case "CHECKPOINT (103)":
                            return HttpResponseStatusOptionsEnum.CHECKPOINT103;
                        case "OK (200)":
                            return HttpResponseStatusOptionsEnum.OK200;
                        case "CREATED (201)":
                            return HttpResponseStatusOptionsEnum.CREATED201;
                        case "ACCEPTED (202)":
                            return HttpResponseStatusOptionsEnum.ACCEPTED202;
                        case "NON_AUTHORITATIVE_INFORMATION (203)":
                            return HttpResponseStatusOptionsEnum.NON_AUTHORITATIVE_INFORMATION203;
                        case "NO_CONTENT (204)":
                            return HttpResponseStatusOptionsEnum.NO_CONTENT204;
                        case "RESET_CONTENT (205)":
                            return HttpResponseStatusOptionsEnum.RESET_CONTENT205;
                        case "PARTIAL_CONTENT (206)":
                            return HttpResponseStatusOptionsEnum.PARTIAL_CONTENT206;
                        case "MULTI_STATUS (207)":
                            return HttpResponseStatusOptionsEnum.MULTI_STATUS207;
                        case "ALREADY_REPORTED (208)":
                            return HttpResponseStatusOptionsEnum.ALREADY_REPORTED208;
                        case "IM_USED (226)":
                            return HttpResponseStatusOptionsEnum.IM_USED226;
                        case "MULTIPLE_CHOICES (300)":
                            return HttpResponseStatusOptionsEnum.MULTIPLE_CHOICES300;
                        case "MOVED_PERMANENTLY (301)":
                            return HttpResponseStatusOptionsEnum.MOVED_PERMANENTLY301;
                        case "FOUND (302)":
                            return HttpResponseStatusOptionsEnum.FOUND302;
                        case "SEE_OTHER (303)":
                            return HttpResponseStatusOptionsEnum.SEE_OTHER303;
                        case "NOT_MODIFIED (304)":
                            return HttpResponseStatusOptionsEnum.NOT_MODIFIED304;
                        case "TEMPORARY_REDIRECT (307)":
                            return HttpResponseStatusOptionsEnum.TEMPORARY_REDIRECT307;
                        case "PERMANENT_REDIRECT (308)":
                            return HttpResponseStatusOptionsEnum.PERMANENT_REDIRECT308;
                        case "BAD_REQUEST (400)":
                            return HttpResponseStatusOptionsEnum.BAD_REQUEST400;
                        case "UNAUTHORIZED (401)":
                            return HttpResponseStatusOptionsEnum.UNAUTHORIZED401;
                        case "PAYMENT_REQUIRED (402)":
                            return HttpResponseStatusOptionsEnum.PAYMENT_REQUIRED402;
                        case "FORBIDDEN (403)":
                            return HttpResponseStatusOptionsEnum.FORBIDDEN403;
                        case "NOT_FOUND (404)":
                            return HttpResponseStatusOptionsEnum.NOT_FOUND404;
                        case "METHOD_NOT_ALLOWED (405)":
                            return HttpResponseStatusOptionsEnum.METHOD_NOT_ALLOWED405;
                        case "NOT_ACCEPTABLE (406)":
                            return HttpResponseStatusOptionsEnum.NOT_ACCEPTABLE406;
                        case "PROXY_AUTHENTICATION_REQUIRED (407)":
                            return HttpResponseStatusOptionsEnum.PROXY_AUTHENTICATION_REQUIRED407;
                        case "REQUEST_TIMEOUT (408)":
                            return HttpResponseStatusOptionsEnum.REQUEST_TIMEOUT408;
                        case "CONFLICT (409)":
                            return HttpResponseStatusOptionsEnum.CONFLICT409;
                        case "GONE (410)":
                            return HttpResponseStatusOptionsEnum.GONE410;
                        case "LENGTH_REQUIRED (411)":
                            return HttpResponseStatusOptionsEnum.LENGTH_REQUIRED411;
                        case "PRECONDITION_FAILED (412)":
                            return HttpResponseStatusOptionsEnum.PRECONDITION_FAILED412;
                        case "PAYLOAD_TOO_LARGE (413)":
                            return HttpResponseStatusOptionsEnum.PAYLOAD_TOO_LARGE413;
                        case "URI_TOO_LONG (414)":
                            return HttpResponseStatusOptionsEnum.URI_TOO_LONG414;
                        case "UNSUPPORTED_MEDIA_TYPE (415)":
                            return HttpResponseStatusOptionsEnum.UNSUPPORTED_MEDIA_TYPE415;
                        case "REQUESTED_RANGE_NOT_SATISFIABLE (416)":
                            return HttpResponseStatusOptionsEnum.REQUESTED_RANGE_NOT_SATISFIABLE416;
                        case "EXPECTATION_FAILED (417)":
                            return HttpResponseStatusOptionsEnum.EXPECTATION_FAILED417;
                        case "I_AM_A_TEAPOT (418)":
                            return HttpResponseStatusOptionsEnum.I_AM_A_TEAPOT418;
                        case "UNPROCESSABLE_ENTITY (422)":
                            return HttpResponseStatusOptionsEnum.UNPROCESSABLE_ENTITY422;
                        case "LOCKED (423)":
                            return HttpResponseStatusOptionsEnum.LOCKED423;
                        case "FAILED_DEPENDENCY (424)":
                            return HttpResponseStatusOptionsEnum.FAILED_DEPENDENCY424;
                        case "TOO_EARLY (425)":
                            return HttpResponseStatusOptionsEnum.TOO_EARLY425;
                        case "UPGRADE_REQUIRED (426)":
                            return HttpResponseStatusOptionsEnum.UPGRADE_REQUIRED426;
                        case "PRECONDITION_REQUIRED (428)":
                            return HttpResponseStatusOptionsEnum.PRECONDITION_REQUIRED428;
                        case "TOO_MANY_REQUESTS (429)":
                            return HttpResponseStatusOptionsEnum.TOO_MANY_REQUESTS429;
                        case "REQUEST_HEADER_FIELDS_TOO_LARGE (431)":
                            return HttpResponseStatusOptionsEnum.REQUEST_HEADER_FIELDS_TOO_LARGE431;
                        case "UNAVAILABLE_FOR_LEGAL_REASONS (451)":
                            return HttpResponseStatusOptionsEnum.UNAVAILABLE_FOR_LEGAL_REASONS451;
                        case "INTERNAL_SERVER_ERROR (500)":
                            return HttpResponseStatusOptionsEnum.INTERNAL_SERVER_ERROR500;
                        case "NOT_IMPLEMENTED (501)":
                            return HttpResponseStatusOptionsEnum.NOT_IMPLEMENTED501;
                        case "BAD_GATEWAY (502)":
                            return HttpResponseStatusOptionsEnum.BAD_GATEWAY502;
                        case "SERVICE_UNAVAILABLE (503)":
                            return HttpResponseStatusOptionsEnum.SERVICE_UNAVAILABLE503;
                        case "GATEWAY_TIMEOUT (504)":
                            return HttpResponseStatusOptionsEnum.GATEWAY_TIMEOUT504;
                        case "HTTP_VERSION_NOT_SUPPORTED (505)":
                            return HttpResponseStatusOptionsEnum.HTTP_VERSION_NOT_SUPPORTED505;
                        case "VARIANT_ALSO_NEGOTIATES (506)":
                            return HttpResponseStatusOptionsEnum.VARIANT_ALSO_NEGOTIATES506;
                        case "INSUFFICIENT_STORAGE (507)":
                            return HttpResponseStatusOptionsEnum.INSUFFICIENT_STORAGE507;
                        case "LOOP_DETECTED (508)":
                            return HttpResponseStatusOptionsEnum.LOOP_DETECTED508;
                        case "BANDWIDTH_LIMIT_EXCEEDED (509)":
                            return HttpResponseStatusOptionsEnum.BANDWIDTH_LIMIT_EXCEEDED509;
                        case "NOT_EXTENDED (510)":
                            return HttpResponseStatusOptionsEnum.NOT_EXTENDED510;
                        case "NETWORK_AUTHENTICATION_REQUIRED (511)":
                            return HttpResponseStatusOptionsEnum.NETWORK_AUTHENTICATION_REQUIRED511;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                public bool IsCONTINUE100()
                {
                    return Value == "CONTINUE (100)";
                }
                public bool IsSWITCHING_PROTOCOLS101()
                {
                    return Value == "SWITCHING_PROTOCOLS (101)";
                }
                public bool IsPROCESSING102()
                {
                    return Value == "PROCESSING (102)";
                }
                public bool IsCHECKPOINT103()
                {
                    return Value == "CHECKPOINT (103)";
                }
                public bool IsOK200()
                {
                    return Value == "OK (200)";
                }
                public bool IsCREATED201()
                {
                    return Value == "CREATED (201)";
                }
                public bool IsACCEPTED202()
                {
                    return Value == "ACCEPTED (202)";
                }
                public bool IsNON_AUTHORITATIVE_INFORMATION203()
                {
                    return Value == "NON_AUTHORITATIVE_INFORMATION (203)";
                }
                public bool IsNO_CONTENT204()
                {
                    return Value == "NO_CONTENT (204)";
                }
                public bool IsRESET_CONTENT205()
                {
                    return Value == "RESET_CONTENT (205)";
                }
                public bool IsPARTIAL_CONTENT206()
                {
                    return Value == "PARTIAL_CONTENT (206)";
                }
                public bool IsMULTI_STATUS207()
                {
                    return Value == "MULTI_STATUS (207)";
                }
                public bool IsALREADY_REPORTED208()
                {
                    return Value == "ALREADY_REPORTED (208)";
                }
                public bool IsIM_USED226()
                {
                    return Value == "IM_USED (226)";
                }
                public bool IsMULTIPLE_CHOICES300()
                {
                    return Value == "MULTIPLE_CHOICES (300)";
                }
                public bool IsMOVED_PERMANENTLY301()
                {
                    return Value == "MOVED_PERMANENTLY (301)";
                }
                public bool IsFOUND302()
                {
                    return Value == "FOUND (302)";
                }
                public bool IsSEE_OTHER303()
                {
                    return Value == "SEE_OTHER (303)";
                }
                public bool IsNOT_MODIFIED304()
                {
                    return Value == "NOT_MODIFIED (304)";
                }
                public bool IsTEMPORARY_REDIRECT307()
                {
                    return Value == "TEMPORARY_REDIRECT (307)";
                }
                public bool IsPERMANENT_REDIRECT308()
                {
                    return Value == "PERMANENT_REDIRECT (308)";
                }
                public bool IsBAD_REQUEST400()
                {
                    return Value == "BAD_REQUEST (400)";
                }
                public bool IsUNAUTHORIZED401()
                {
                    return Value == "UNAUTHORIZED (401)";
                }
                public bool IsPAYMENT_REQUIRED402()
                {
                    return Value == "PAYMENT_REQUIRED (402)";
                }
                public bool IsFORBIDDEN403()
                {
                    return Value == "FORBIDDEN (403)";
                }
                public bool IsNOT_FOUND404()
                {
                    return Value == "NOT_FOUND (404)";
                }
                public bool IsMETHOD_NOT_ALLOWED405()
                {
                    return Value == "METHOD_NOT_ALLOWED (405)";
                }
                public bool IsNOT_ACCEPTABLE406()
                {
                    return Value == "NOT_ACCEPTABLE (406)";
                }
                public bool IsPROXY_AUTHENTICATION_REQUIRED407()
                {
                    return Value == "PROXY_AUTHENTICATION_REQUIRED (407)";
                }
                public bool IsREQUEST_TIMEOUT408()
                {
                    return Value == "REQUEST_TIMEOUT (408)";
                }
                public bool IsCONFLICT409()
                {
                    return Value == "CONFLICT (409)";
                }
                public bool IsGONE410()
                {
                    return Value == "GONE (410)";
                }
                public bool IsLENGTH_REQUIRED411()
                {
                    return Value == "LENGTH_REQUIRED (411)";
                }
                public bool IsPRECONDITION_FAILED412()
                {
                    return Value == "PRECONDITION_FAILED (412)";
                }
                public bool IsPAYLOAD_TOO_LARGE413()
                {
                    return Value == "PAYLOAD_TOO_LARGE (413)";
                }
                public bool IsURI_TOO_LONG414()
                {
                    return Value == "URI_TOO_LONG (414)";
                }
                public bool IsUNSUPPORTED_MEDIA_TYPE415()
                {
                    return Value == "UNSUPPORTED_MEDIA_TYPE (415)";
                }
                public bool IsREQUESTED_RANGE_NOT_SATISFIABLE416()
                {
                    return Value == "REQUESTED_RANGE_NOT_SATISFIABLE (416)";
                }
                public bool IsEXPECTATION_FAILED417()
                {
                    return Value == "EXPECTATION_FAILED (417)";
                }
                public bool IsI_AM_A_TEAPOT418()
                {
                    return Value == "I_AM_A_TEAPOT (418)";
                }
                public bool IsUNPROCESSABLE_ENTITY422()
                {
                    return Value == "UNPROCESSABLE_ENTITY (422)";
                }
                public bool IsLOCKED423()
                {
                    return Value == "LOCKED (423)";
                }
                public bool IsFAILED_DEPENDENCY424()
                {
                    return Value == "FAILED_DEPENDENCY (424)";
                }
                public bool IsTOO_EARLY425()
                {
                    return Value == "TOO_EARLY (425)";
                }
                public bool IsUPGRADE_REQUIRED426()
                {
                    return Value == "UPGRADE_REQUIRED (426)";
                }
                public bool IsPRECONDITION_REQUIRED428()
                {
                    return Value == "PRECONDITION_REQUIRED (428)";
                }
                public bool IsTOO_MANY_REQUESTS429()
                {
                    return Value == "TOO_MANY_REQUESTS (429)";
                }
                public bool IsREQUEST_HEADER_FIELDS_TOO_LARGE431()
                {
                    return Value == "REQUEST_HEADER_FIELDS_TOO_LARGE (431)";
                }
                public bool IsUNAVAILABLE_FOR_LEGAL_REASONS451()
                {
                    return Value == "UNAVAILABLE_FOR_LEGAL_REASONS (451)";
                }
                public bool IsINTERNAL_SERVER_ERROR500()
                {
                    return Value == "INTERNAL_SERVER_ERROR (500)";
                }
                public bool IsNOT_IMPLEMENTED501()
                {
                    return Value == "NOT_IMPLEMENTED (501)";
                }
                public bool IsBAD_GATEWAY502()
                {
                    return Value == "BAD_GATEWAY (502)";
                }
                public bool IsSERVICE_UNAVAILABLE503()
                {
                    return Value == "SERVICE_UNAVAILABLE (503)";
                }
                public bool IsGATEWAY_TIMEOUT504()
                {
                    return Value == "GATEWAY_TIMEOUT (504)";
                }
                public bool IsHTTP_VERSION_NOT_SUPPORTED505()
                {
                    return Value == "HTTP_VERSION_NOT_SUPPORTED (505)";
                }
                public bool IsVARIANT_ALSO_NEGOTIATES506()
                {
                    return Value == "VARIANT_ALSO_NEGOTIATES (506)";
                }
                public bool IsINSUFFICIENT_STORAGE507()
                {
                    return Value == "INSUFFICIENT_STORAGE (507)";
                }
                public bool IsLOOP_DETECTED508()
                {
                    return Value == "LOOP_DETECTED (508)";
                }
                public bool IsBANDWIDTH_LIMIT_EXCEEDED509()
                {
                    return Value == "BANDWIDTH_LIMIT_EXCEEDED (509)";
                }
                public bool IsNOT_EXTENDED510()
                {
                    return Value == "NOT_EXTENDED (510)";
                }
                public bool IsNETWORK_AUTHENTICATION_REQUIRED511()
                {
                    return Value == "NETWORK_AUTHENTICATION_REQUIRED (511)";
                }
            }

            public enum HttpResponseStatusOptionsEnum
            {
                CONTINUE100,
                SWITCHING_PROTOCOLS101,
                PROCESSING102,
                CHECKPOINT103,
                OK200,
                CREATED201,
                ACCEPTED202,
                NON_AUTHORITATIVE_INFORMATION203,
                NO_CONTENT204,
                RESET_CONTENT205,
                PARTIAL_CONTENT206,
                MULTI_STATUS207,
                ALREADY_REPORTED208,
                IM_USED226,
                MULTIPLE_CHOICES300,
                MOVED_PERMANENTLY301,
                FOUND302,
                SEE_OTHER303,
                NOT_MODIFIED304,
                TEMPORARY_REDIRECT307,
                PERMANENT_REDIRECT308,
                BAD_REQUEST400,
                UNAUTHORIZED401,
                PAYMENT_REQUIRED402,
                FORBIDDEN403,
                NOT_FOUND404,
                METHOD_NOT_ALLOWED405,
                NOT_ACCEPTABLE406,
                PROXY_AUTHENTICATION_REQUIRED407,
                REQUEST_TIMEOUT408,
                CONFLICT409,
                GONE410,
                LENGTH_REQUIRED411,
                PRECONDITION_FAILED412,
                PAYLOAD_TOO_LARGE413,
                URI_TOO_LONG414,
                UNSUPPORTED_MEDIA_TYPE415,
                REQUESTED_RANGE_NOT_SATISFIABLE416,
                EXPECTATION_FAILED417,
                I_AM_A_TEAPOT418,
                UNPROCESSABLE_ENTITY422,
                LOCKED423,
                FAILED_DEPENDENCY424,
                TOO_EARLY425,
                UPGRADE_REQUIRED426,
                PRECONDITION_REQUIRED428,
                TOO_MANY_REQUESTS429,
                REQUEST_HEADER_FIELDS_TOO_LARGE431,
                UNAVAILABLE_FOR_LEGAL_REASONS451,
                INTERNAL_SERVER_ERROR500,
                NOT_IMPLEMENTED501,
                BAD_GATEWAY502,
                SERVICE_UNAVAILABLE503,
                GATEWAY_TIMEOUT504,
                HTTP_VERSION_NOT_SUPPORTED505,
                VARIANT_ALSO_NEGOTIATES506,
                INSUFFICIENT_STORAGE507,
                LOOP_DETECTED508,
                BANDWIDTH_LIMIT_EXCEEDED509,
                NOT_EXTENDED510,
                NETWORK_AUTHENTICATION_REQUIRED511
            }
        }

    }
}