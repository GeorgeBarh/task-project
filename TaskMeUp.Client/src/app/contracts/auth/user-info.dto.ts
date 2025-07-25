import { PartialGroup } from '../group/partial-group';

export interface UserInfoDto {
  username: string;
  portrait: string;
  token: string;
  groups: PartialGroup[];
}
