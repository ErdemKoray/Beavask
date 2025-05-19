export interface CompanyProfile {
  companyId: number;
  companyName: string;
  companyEmail: string;
  companyPhone?: string | null;
  companyDescription?: string | null;
  companyWebsite?: string | null;
  companyLogoUrl?: string | null;
}
