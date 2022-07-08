const sut = require("../../helpers/createQueryStringSnippet");

test("createQueryString should not be null", () => {
    expect(sut).not.toBeNull();
});

test("object parameter", () => {
    const objectParam = {
        name: "value",
        in: "query",
        schema: {
            type: "object",
            properties: {
                id: { type: "integer" },
                type: { type: "string" }
            }
        }
    };

    expect(sut([objectParam])).toBe(`var queryString = new StringBuilder();
if(value != null)
{ queryString.Append($"{(queryString.Length == 0 ? "?" : "&")}id={value.id}&{(value.type == null ? string.Empty : "type=" + Uri.EscapeDataString(value.type))}"); }`);
});
