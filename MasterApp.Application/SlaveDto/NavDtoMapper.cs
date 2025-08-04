namespace MasterApp.Application.SlaveDto;

public static class NavDtoMapper
{
    public static CreateNavCloudPosDBKMARTDto ToDto(this CreateNavInputDto input)
    {
        return new CreateNavCloudPosDBKMARTDto
        {
            SERIAL = input.serial,
            PARENT_ID = input.parenT_ID ?? 0,
            DESCRIPTION = input.description,
            URL = input.url,
            PER_ROLE = input.peR_ROLE,
            ENTRY_BY = input.entrY_BY,
            ENTRY_DATE = input.entrY_DATE,
            ORDER_BY = input.ordeR_BY,
            FA_CLASS = input.fA_CLASS,
            ID = input.id,
            MENU_TYPE = input.menU_TYPE,
            SHOW_EDIT_PERMISSION = input.shoW_EDIT_PERMISSION
        };
    }
}
