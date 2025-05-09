import { Company } from "../../model/company.model";
import { Customer } from "../../model/customer.model";
import { User } from "../../model/user.model";

export interface ProjectDetail {
  id: number;
    name: string;
    description: string;
    repoUrl: string;
    createdAt: Date;
    updatedAt: Date | null;
    isActive: boolean;
    isCompanyProject: boolean;
    userId: number;
    user: User | null;
    companyId: number | null;
    company: Company | null;
    customerId: number | null;
    customer: Customer | null;
}
