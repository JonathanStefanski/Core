
export class UserRoles {
    public static readonly ADMIN = 'Admin';
    public static readonly MODERATOR = 'Moderator';
    public static readonly MEMBER = 'Member';
    public static readonly VIP = 'VIP';

    static GetRoles(): any[] {
        return [
            { name: UserRoles.ADMIN, value: UserRoles.ADMIN },
            { name: UserRoles.MODERATOR, value: UserRoles.MODERATOR },
            { name: UserRoles.MEMBER, value: UserRoles.MEMBER },
            { name: UserRoles.VIP, value: UserRoles.VIP }
        ];
    }
}




